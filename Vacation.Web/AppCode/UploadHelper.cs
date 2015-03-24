using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using Common.Models;
using System.Web.Caching;
using System.Xml.Serialization;

namespace Vacation.Web.AppCode
{
    public class UploadHelper
    {
        static UploadHelper()
        {
            UploadSettings.Init();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">HttpPostedFile</param>
        /// <param name="saveDir">保存的目录</param>
        /// <param name="saveFileName">保存的文件名，默认为：yyyyMMddhhmmssfff</param>
        /// <returns></returns> 
        /// <exception cref="System.ArgumentNullException">file</exception>
        public static VMessage UploadFile(HttpPostedFileBase file, string saveDir, string saveFileName = null)
        {
            //HttpPostedFile file = HttpContext.Current.Request.Files["Filedata"];

            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if (string.IsNullOrEmpty(saveDir))
            {
                throw new ArgumentNullException("saveDir");
            }

            VMessage result = new VMessage();

            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }

            string ext = System.IO.Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(saveFileName))
            {
                saveFileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + ext;
            }

            //判断文件格式是否是禁传格式
            List<string> ignoreExtensions = UploadSettings.Instance.Ignore.Extensions;

            if (ignoreExtensions.Contains(ext, new IgnoreCaseEqualityComparer()))
            {
                result.AddItem("上传格式不正确");
            }
            else
            {
                string saveFilePath = Path.Combine(saveDir, saveFileName);   //绝对路径 

                file.SaveAs(saveFilePath);

                result.Entity = saveFileName;
            }

            return result;
        }

        /// <summary>
        /// 取消文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param> 
        /// <exception cref="System.ArgumentNullException">filePath</exception>
        /// <exception cref="System.IO.FileNotFoundException">filePath</exception>
        public static void CancelFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("filePath");
            }

            File.Delete(filePath);
        }

        /// <summary>
        /// 转移为<c>FileTypeDescription</c>
        /// </summary>
        /// <param name="fileType">默认文件格式名或文件格式</param>
        /// <param name="fileTypeDesc">文件格式描述，默认时可为空</param>
        /// <returns></returns>
        /// 创建人：尹学良
        /// 2014/11/21 14:27
        public static FileTypeDescription ConvertTo(string fileType, string fileTypeDesc)
        {
            FileTypeDescription result = new FileTypeDescription();

            var _default = UploadSettings.Instance.Defaults.FirstOrDefault(d => d.FileType == fileType);

            if (_default == null)
            {
                result.FileTypeExts = fileType;
                result.FileTypeDesc = fileTypeDesc;
            }
            else
            {
                result.FileTypeExts = string.Join(";", _default.Extensions);
                result.FileTypeDesc = _default.FileTypeDesc;
            }
            return result;
        }
    }

    /// <summary>
    ///忽略大小写比较
    /// </summary> 
    public class IgnoreCaseEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            return x.ToLower() == y.ToLower();
        }

        public int GetHashCode(string obj)
        {
            return obj.ToLower().GetHashCode();
        }
    }

    /// <summary>
    /// 上传控件配置类
    /// </summary>
    [XmlRoot("settings")]
    public class UploadSettings
    {
        /// <summary>
        /// 缓存Key
        /// </summary>
        private static readonly string _cacheKey = "UploadSettings";

        private static UploadSettings _instance;

        public static UploadSettings Instance { get { return _instance; } }

        private UploadSettings() { }

        #region Public Properties

        [XmlElement("ignore")]
        public IgnoreExtensions Ignore { get; set; }

        [XmlElement("default")]
        public List<DefaultTypeExtensions> Defaults { get; set; }

        #endregion

        private static int s_UploadSettingsCacheDependencyFlag = 0;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            string path = GetSettingsFilePath();

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("上传控件的配置文件不存在！");
            }

            // 注意啦：访问文件是可能会出现异常。不要学我，我写的是示例代码。
            var settings = Deserialize(path);

            int flag = System.Threading.Interlocked.CompareExchange(ref s_UploadSettingsCacheDependencyFlag, 1, 0);

            // 确保只调用一次就可以了。
            if (flag == 0)
            {
                // 让Cache帮我们盯住这个配置文件。
                CacheDependency dep = new CacheDependency(path);
                HttpRuntime.Cache.Insert(_cacheKey, "yxl", dep, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, UploadSettingsUpdateCallback);
            }

            _instance = settings;
        }

        private static void UploadSettingsUpdateCallback(
            string key, CacheItemUpdateReason reason,
            out object expensiveObject,
            out CacheDependency dependency,
            out DateTime absoluteExpiration,
            out TimeSpan slidingExpiration)
        {
            // 注意哦：在这个方法中，不要出现【未处理异常】，否则缓存对象将被移除。

            // 说明：这里我并不关心参数reason，因为我根本就没有使用过期时间
            //        所以，只有一种原因：依赖的文件发生了改变。
            //        参数key我也不关心，因为这个方法是【专用】的。

            expensiveObject = "yxl";
            dependency = new CacheDependency(GetSettingsFilePath());
            absoluteExpiration = Cache.NoAbsoluteExpiration;
            slidingExpiration = Cache.NoSlidingExpiration;

            // 重新加载配置参数
            Init();
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <returns></returns>
        private static string GetSettingsFilePath()
        {
            // 默认为/App_Data/UploadSettings.xml
            string path = System.Configuration.ConfigurationManager.AppSettings["UploadSettingsPath"] ?? "/App_Data/UploadSettings.xml";

            // 需要相对路径
            path = path.TrimStart('/').TrimStart('\\');

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }

        private static UploadSettings Deserialize(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UploadSettings));

            return (UploadSettings)serializer.Deserialize(stream);
        }

        private static UploadSettings Deserialize(string filePath)
        {
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return Deserialize(file);
            }
        }

        /// <summary>
        /// 忽略类型
        /// </summary>
        public class IgnoreExtensions
        {
            [XmlElement("ext")]
            public List<string> Extensions { get; set; }
        }

        /// <summary>
        /// 内置类型
        /// </summary> 
        public class DefaultTypeExtensions
        {
            [XmlElement("ext")]
            public List<string> Extensions { get; set; }

            [XmlAttribute("type")]
            public string FileType { get; set; }

            [XmlAttribute("desc")]
            public string FileTypeDesc { get; set; }
        }
    }

    /// <summary>
    /// 文件类型描述
    /// </summary>
    /// <remarks>
    /// 用于uploadify
    /// </remarks>
    public class FileTypeDescription
    {
        [XmlAttribute("type")]
        public string FileTypeExts { get; set; }

        [XmlAttribute("desc")]
        public string FileTypeDesc { get; set; }
    }
}
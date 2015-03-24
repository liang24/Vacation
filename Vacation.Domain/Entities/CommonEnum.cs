using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.EnumLib;

namespace Vacation.Domain.Entities
{
    public enum MasterType
    {
        User,
        Role
    }

    [Flags]
    public enum PreDefinedFunction
    {
        [Enum("新增")]
        add = 2,
        [Enum("删除")]
        delete = 8,
        [Enum("导入")]
        import = 0x10,
        [Enum("修改")]
        update = 4,
        [Enum("查看")]
        view = 1
    }
}

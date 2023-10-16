using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Common.Exceptions
{
    public class DomainObjectInUseExeption : Exception
    {
        public DomainObjectInUseExeption()
            : base()
        {
        }

        public DomainObjectInUseExeption(string message)
            : base(message)
        {
        }

        public DomainObjectInUseExeption(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DomainObjectInUseExeption(string parentName, object parentKey, string childName, IEnumerable<object> childKeys)
            : base($"Entity \"{parentName}\" ({parentKey}) is still in use by \"{childName}\" ({childKeys.Aggregate((keys, key) => $"{keys},{key}")})")
        {
        }
    }
}

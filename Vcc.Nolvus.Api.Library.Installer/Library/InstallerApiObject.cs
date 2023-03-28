using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Api.Installer.Library
{
    public abstract class InstallerApiObject
    {
        public static void CopyProperties(object objSource, object objDestination)
        {            
            var destProps = objDestination.GetType().GetProperties();
            
            foreach (var sourceProp in objSource.GetType().GetProperties())
            {
                foreach (var destProperty in destProps)
                {                    
                    if (destProperty.Name == sourceProp.Name &&
                            destProperty.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                    {
                        destProperty.SetValue(objDestination, sourceProp.GetValue(objSource));
                        break;
                    }
                }
            }
        }


        public T Dump<T>()
        {            
            T Result = Activator.CreateInstance<T>();

            InstallerApiObject.CopyProperties(this, Result);

            return Result;
        }      
    }

    public interface IInstallerApiDTO
    {
        string Id { get; }
        T Dump<T>();
    }
}

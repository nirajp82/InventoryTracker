using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CopyIgnoreAttribute : System.Attribute
    {
    }
}

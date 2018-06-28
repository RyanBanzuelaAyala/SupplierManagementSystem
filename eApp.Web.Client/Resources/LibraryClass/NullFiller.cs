using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace eApp.Web.Client.Resources.LibraryClass
{
    public static class NullFiller
    {
        public static void FillNullFields<T>(T theInstance)
        {
            if (theInstance == null)
            {
                throw new ArgumentNullException();
            }


            foreach (var property in theInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var theValue = property.GetValue(theInstance, null);

                if (theValue == null || theValue.ToString() == "")
                {
                    property.SetValue(theInstance, ("0"), null);
                }
            }
        }

        public static void FillNullFields<T>(IEnumerable<T> theInstance)
        {
            if (theInstance == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var theItem in theInstance)
            {
                FillNullFields(theItem);
            }
        }
    }
}
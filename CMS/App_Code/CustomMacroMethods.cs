using CMS;
using CMS.MacroEngine;
using CMS.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.FormEngine;
//using CMS.GlobalHelper;
//using CMS.SettingsProvider;
using System.Collections;
using CMS.DataEngine;
//using CMS.CMSHelper;

// Makes all methods in the 'CustomMacroMethods' container class available for string objects
[assembly: RegisterExtension(typeof(CustomMacroMethods), typeof(string))]
// Registers methods from the 'CustomMacroMethods' container into the "String" macro namespace
[assembly: RegisterExtension(typeof(CustomMacroMethods), typeof(StringNamespace))]

public class CustomMacroMethods : MacroMethodContainer
{
    [MacroMethod(typeof(string), "Get the text of a list field (like checkbox, radio button or dropdownlist), instead of the value", 1)]
    [MacroMethodParam(0, "param1", typeof(object), "Field name inside quotes e.g. \"CountryDropDownList\" ")]
    [MacroMethodParam(1, "param2", typeof(object), "ClassID found in CMS_Form table e.g. 1234")]
    [MacroMethodParam(2, "param2", typeof(object), "Field name without quotes e.g. CountryDropDownList")]

    public static object GetFormfieldText(EvaluationContext context, params object[] parameters)
    {
        object fieldName = ValidationHelper.GetString(parameters[0], "");
        object nodeClassId = ValidationHelper.GetString(parameters[1], "");
        object id = ValidationHelper.GetString(parameters[2], "");

        DataClassInfo classInfo = DataClassInfoProvider.GetDataClassInfo(ValidationHelper.GetInteger(nodeClassId, 0));

        if (classInfo != null)
        {
            FormInfo formInfo = new FormInfo(classInfo.ClassFormDefinition);
            FormFieldInfo ffi = formInfo.GetFormField(ValidationHelper.GetString(fieldName, ""));

            if (ffi != null)
            {
                List<string> optionsList = ffi.Settings["Options"].ToString().Split('\n').ToList();

                optionsList = optionsList.Select(s => s = s.Trim()).ToList();

                Hashtable options = new Hashtable();
                optionsList.ForEach(s => options.Add(s.Split(';')[0], s.Split(';')[1]));

                if (options[id] != null)
                {
                    return options[id].ToString();
                }
            }
        }

        return "";
    }
}

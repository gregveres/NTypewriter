﻿using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NTypewriter.Internals;
using Scriban;
using Scriban.Syntax;



namespace NTypewriter
{
    public class NTypeWriter
    {
        public static async Task<Result> Render(string template, object dataModel, Configuration configuration = null)
        {
            var result = new Result();
            var scribanTemplate = Template.Parse(template);

            result.AddMsgFromScribanTemplateParsing(scribanTemplate.Messages);

            if (result.HasErrors)
            {
                return result;            
            }          
           
            var mainScriptObject = new MainScriptObject(dataModel);
            var userScriptObject = new CustomFunctionsScriptObject(configuration?.typesWithCustomFuntions);
            var context = new MainTemplateContext(mainScriptObject, userScriptObject);

            try
            {
                await scribanTemplate.RenderAsync(context);
            }
            catch (ScriptRuntimeException exception)
            {
                result.AddMsgFromScribanException(exception);
            }

            if (result.HasErrors)
            {
                return result;
            }

            result.AddRenderedItems(context.GetRenderedItems());

            return result;
        }
    }
}
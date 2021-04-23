using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTypewriter.CodeModel.Roslyn;
using System.Linq;
using System.Threading.Tasks;

namespace NTypewriter.CodeModel.Functions.Tests.Method
{
    [TestClass]
    public class ActionFunctionsTests_WebApi20 : BaseFixture
    {
        ICodeModel data;
        Configuration settings;

        [TestInitialize]
        public async Task Initialize()
        {
            var config = new CodeModelConfiguration().FilterByNamespace("Tests.Assets.MvcWebApi2");
            data = await CreateCodeModelFromProject(config, "Tests.Assets.MvcWebApi2");
            settings = new Configuration();
        }


        [TestMethod]
        public async Task HttpMethod()
        {
            var template = @"{{- capture result }}
                                {{- for class in data.Classes | Types.ThatInheritFrom ""ApiController"" }}
                                   {{- for method in class.Methods }}                            
                                      [{{- method.Name }} : {{- method | Action.HttpMethod }}]
                                   {{- end }}
                                {{- end }}
                             {{- end }}
                             {{- Save result ""Some name"" }}
                            ";
            var result = await NTypeWriter.Render(template, data, settings);
            var actual = RemoveWhitespace(result.Items.First().Content);

            var expected = "[SyncMethodIntReturn:get][AsyncMethodIntReturn:post][SyncMethodComplexReturn:put][AsyncMethodComplexReturn:delete][SyncMethodListReturn:get][AsyncMethodListReturn:get]";
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public async Task BodyParameter()
        {
            var template = @"{{- capture result }}
                                {{- for class in data.Classes | Types.ThatInheritFrom ""ApiController"" }}
                                   {{- for method in class.Methods }}                            
                                      [{{- method.Name }} : {{- method | Action.BodyParameter }}]
                                   {{- end }}
                                {{- end }}
                             {{- end }}
                             {{- Save result ""Some name"" }}
                            ";
            var result = await NTypeWriter.Render(template, data, settings);
            var actual = RemoveWhitespace(result.Items.First().Content);

            // TODO: this is wrong
            var expected = "[SyncMethodIntReturn:][AsyncMethodIntReturn:ComplexReturnbody][SyncMethodComplexReturn:][AsyncMethodComplexReturn:][SyncMethodListReturn:][AsyncMethodListReturn:EnumTypearg2]";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task UrlParameters()
        {
            var template = @"{{- capture result }}
                                {{- for class in data.Classes | Types.ThatInheritFrom ""ApiController"" }}
                                   {{- for method in class.Methods }}                            
                                      [{{- method.Name }} : {{- method | Action.UrlParameters }}]
                                   {{- end }}
                                {{- end }}
                             {{- end }}
                             {{- Save result ""Some name"" }}
                            ";
            var result = await NTypeWriter.Render(template, data, settings);
            var actual = RemoveWhitespace(result.Items.First().Content);

            // TODO: this is wrong
            var expected = "[SyncMethodIntReturn:[intid]][AsyncMethodIntReturn:[intid,stringname]][SyncMethodComplexReturn:[intid,stringname,stringsubname]][AsyncMethodComplexReturn:[intid]][SyncMethodListReturn:[intid,stringname,stringarg]][AsyncMethodListReturn:[intid,stringarg]]";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Url()
        {
            var template = @"{{- capture result
                                 for class in data.Classes | Types.ThatInheritFrom ""ApiController"" 
                                     for method in class.Methods }}                       
                                      [{{- method.Name }} : {{- method | Action.Url }}]
                                   {{- end
                                 end 
                                 end 
                                 Save result ""Some name"" }}
                            ";
            var result = await NTypeWriter.Render(template, data, settings);
            var actual = RemoveWhitespace(result.Items.First().Content);

            // TODO: this is wrong
            var expected = "[SyncMethodIntReturn:?id=${id}][AsyncMethodIntReturn:?id=${id}&name=${encodeURIComponent(name)}][SyncMethodComplexReturn:?id=${id}&name=${encodeURIComponent(name)}&subname=${encodeURIComponent(subname)}][AsyncMethodComplexReturn:?id=${id}][SyncMethodListReturn:?id=${id}&name=${encodeURIComponent(name)}&arg=${encodeURIComponent(arg)}][AsyncMethodListReturn:?id=${id}&arg=${encodeURIComponent(arg)}]";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ReturnType()
        {
            var template = @"{{- capture result
                                 for class in data.Classes | Types.ThatInheritFrom ""ApiController"" 
                                     for method in class.Methods }}                       
                                      [{{- method.Name }} : {{- method | Action.ReturnType }}]
                                   {{- end
                                 end 
                                 end 
                                 Save result ""Some name"" }}
                            ";
            var result = await NTypeWriter.Render(template, data, settings);
            var actual = RemoveWhitespace(result.Items.First().Content);

            var expected = "[SyncMethodIntReturn:int][AsyncMethodIntReturn:int][SyncMethodComplexReturn:ComplexReturn][AsyncMethodComplexReturn:ComplexReturn][SyncMethodListReturn:List<ComplexReturn>][AsyncMethodListReturn:List<ComplexReturn>]";

            Assert.AreEqual(expected, actual);
        }


    }
}

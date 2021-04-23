using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Tests.Assets.WebApiMVC20.Controllers
{
    [RoutePrefix("api/Home/{id}")]
    public class HomeController : ApiController
    {
        [HttpGet]
        [Route(Name = "NamedRoute")]
        [ResponseType(typeof(int))]
        public IHttpActionResult SyncMethodIntReturn(int id)
        {
            return Ok(id);
        }

        [HttpPost]
        [Route("{name}")]
        [ResponseType(typeof(int))]
        public Task<IHttpActionResult> AsyncMethodIntReturn(int id, string name, ComplexReturn body)
        {
            return Task.FromResult<IHttpActionResult>(Ok(id));
        }

        [HttpPut]
        [Route("{name}/{subname}")]
        [ResponseType(typeof(ComplexReturn))]
        public IHttpActionResult SyncMethodComplexReturn(int id, string name, string subname)
        {
            return Ok(new ComplexReturn());
        }

        [HttpDelete]
        [Route("complex")]
        [ResponseType(typeof(ComplexReturn))]
        public Task<IHttpActionResult> AsyncMethodComplexReturn(int id)
        {
            return Task.FromResult<IHttpActionResult>(Ok(new ComplexReturn()));
        }

        [HttpGet]
        [Route("list/{name}")]
        [ResponseType(typeof(List<ComplexReturn>))]
        public IHttpActionResult SyncMethodListReturn(int id, string name, string arg)
        {
            return Ok(new ComplexReturn());
        }

        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(List<ComplexReturn>))]
        public Task<IHttpActionResult> AsyncMethodListReturn(int id, string arg, EnumType arg2)
        {
            return Task.FromResult<IHttpActionResult>(Ok(new ComplexReturn()));
        }
    }

    public class ComplexReturn
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public enum EnumType
    {
        no = 0,
        yes = 1
    }
}

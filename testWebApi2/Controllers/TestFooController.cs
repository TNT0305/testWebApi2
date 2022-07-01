using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testWebApi2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestFooController : ControllerBase
    {
        private readonly ILogger<TestFooController> _logger;
        TestWrapper.Foo _fooInstance;
        public TestFooController(ILogger<TestFooController> logger, TestWrapper.Foo fooInstance)
        {
            _logger = logger;
            _fooInstance = fooInstance;
        }


        /// <summary>
        /// call the global function which returns val+1
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("testNativeFoo")]
        public int TestNativeFoo(int val)
        {
            _logger?.LogInformation("TestNativeFoo received: {val}", val);
            return TestWrapper.MyLibWrapper.fooPublicFunction(val);
        }

        /// <summary>
        /// Test use of intel threading building blocks from the shared library
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("parallelSum")]
        public Int64 ParallelSum(int val)
        {
            _logger?.LogInformation("ParallelSum received: {val}", val);
            return TestWrapper.MyLibWrapper.ParallelSum(val);
        }

        /// <summary>
        /// Take the singleton foo instance, add 1 to its current value and return the new value
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("nextFooVal")]
        public int NextFooVal()
        {
            _fooInstance.SetA(_fooInstance.GetA() + 1);
            var rv = _fooInstance.GetA();
            _logger?.LogInformation("NextFooVal set to : {val}", rv);
            return rv;
        }
    }
}

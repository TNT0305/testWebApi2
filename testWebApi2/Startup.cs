using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace testWebApi2
{
    public class Startup
    {
        public enum MachineType : ushort
        {
            IMAGE_FILE_MACHINE_UNKNOWN = 0x0,
            IMAGE_FILE_MACHINE_AM33 = 0x1d3,
            IMAGE_FILE_MACHINE_AMD64 = 0x8664,
            IMAGE_FILE_MACHINE_ARM = 0x1c0,
            IMAGE_FILE_MACHINE_EBC = 0xebc,
            IMAGE_FILE_MACHINE_I386 = 0x14c,
            IMAGE_FILE_MACHINE_IA64 = 0x200,
            IMAGE_FILE_MACHINE_M32R = 0x9041,
            IMAGE_FILE_MACHINE_MIPS16 = 0x266,
            IMAGE_FILE_MACHINE_MIPSFPU = 0x366,
            IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466,
            IMAGE_FILE_MACHINE_POWERPC = 0x1f0,
            IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1,
            IMAGE_FILE_MACHINE_R4000 = 0x166,
            IMAGE_FILE_MACHINE_SH3 = 0x1a2,
            IMAGE_FILE_MACHINE_SH3DSP = 0x1a3,
            IMAGE_FILE_MACHINE_SH4 = 0x1a6,

            IMAGE_FILE_MACHINE_SH5 = 0x1a8,
            IMAGE_FILE_MACHINE_THUMB = 0x1c2,
            IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x169,
            IMAGE_FILE_MACHINE_ARM64 = 0xaa64
        }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //int bits = IntPtr.Size * 8;
            //var mt = GetDllMachineType(@"C:\Users\TroyBerg\source\repos\testWebApi2\TestWrapper\native\out\build\x64-debug\testSharedLibrary\testSharedLibrary.dll");
            //System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();

            var rv = TestWrapper.MyLibWrapper.fooPublicFunction(1);
            using (var f = new TestWrapper.Foo())
            {
                f.SetA(27);
                Console.WriteLine(f.GetA());
            }
                
            Console.WriteLine(rv);
            //rv.Should().NotBeNull();
        }
        private static string GetXmlCommentPath(string assemblyName) => Path.Combine(AppContext.BaseDirectory, assemblyName + ".xml");
        public static MachineType GetDllMachineType(string dllPath)
        {
            // See http://www.microsoft.com/whdc/system/platform/firmware/PECOFF.mspx
            // Offset to PE header is always at 0x3C.
            // The PE header starts with "PE\0\0" =  0x50 0x45 0x00 0x00,
            // followed by a 2-byte machine type field (see the document above for the enum).
            //
            using (var fs = new FileStream(dllPath, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            {
                fs.Seek(0x3c, SeekOrigin.Begin);
                Int32 peOffset = br.ReadInt32();

                fs.Seek(peOffset, SeekOrigin.Begin);
                UInt32 peHead = br.ReadUInt32();

                if (peHead != 0x00004550) // "PE\0\0", little-endian
                    throw new Exception("Can't find PE header");

                return (MachineType)br.ReadUInt16();
            }
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSingleton<TestWrapper.Foo>();

            services.AddEndpointsApiExplorer();
            //services.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Foo Testing API",
                    Description = "A Web API for testing calling a .net wrapper to a cross platform shared library",

                });
                //options.OperationFilter<FileResultContentTypeOperationFilter>();
                //options.SchemaFilter<EnumSchemaFilter>();


                // using System.Reflection;
                var xmlFilename = GetXmlCommentPath(Assembly.GetExecutingAssembly().GetName().Name);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                //Collect all referenced projects output XML document file paths  
                var currentAssembly = Assembly.GetExecutingAssembly();
                var xmlDocs = currentAssembly.GetReferencedAssemblies()
                    .Union(new AssemblyName[] { currentAssembly.GetName() })
                    .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))
                    .Where(f => File.Exists(f)).ToArray();
                Array.ForEach(xmlDocs, (d) =>
                {
                    options.IncludeXmlComments(d);
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "testWebApi2 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

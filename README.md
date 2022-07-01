# testWebApi2

## TL;DR
- Build a C++ shared libary using cmake in vs2022 for linux and windows
- Wrap the native shared library in a dotnet class libary using P/Invoke
- Consume the wrapper library in a dotnet webApi project
- Observe the webapi project calling methods from the C++ shared library in the browser

## Description
This is a project space to work out a good development workflow for a C++ shared library using CMake and consuming it from a .NET 6 context.  Inspired by my own SO question, [here](https://stackoverflow.com/questions/72816655/cross-platform-dotnet-6-and-c).

This is a straw-man github repo and comments, suggestions, and PRs are welcome.

### Build steps
On the initial commit, the workflow is a bit cumbersome.  
- Native Shared Libraries
  - In vs2022: "Open CMake..." the "./native" folder
  - Build release versions for windows and linux.
- Wrapper and consuming app
  - Open the testWebApi2.sln
  - Build the solution.
    - Building at this time is necessary as TestWrapper.csproj has a post-build step to create a nuget package referenced by testWebApi2.csproj

At this point, you should be able to Debug the testWebApi2 in windows and in a linux docker container.

### nuget
To allow both windows and linux versions to compile without having to install the library (and native shared librar) in each environment, we're creating a multi-target nuget package. This package is generated in a post-build step in TestWrapper.

The post build step in TestWrapper.csproj looks like:
~~~xml
  <Target Name="Pack nuget package" AfterTargets="Build">
    <Message Importance="high" Text="Packing TestWrapper into nuget package" />
    <Exec Command="dotnet pack --no-build -c:Release -p:NuspecFile=TestWrapper.nuspec --output ../nupkgs"/>
  </Target>
~~~

The TestWrapper.nuspec is including, explicitly, the linux and windows shared libraries (from ./native cmake project) as well as the TestWrapper.dll (and .pdb)

The testWebApi2.csproj includes a nuget.config that instructs the dotnet compiler to reference the ../nupkgs to find the TestWrapper.

## Dockerfile
At this point, the dockerfile is not much beyond the default.

Into the base (runtime) image, we install the intel threading building blocks runtime
~~~dockerfile
# install runtime dependencies, (libtbb2 - intel threading building blocks)
RUN set -ex;         \
    apt-get update;  \
    apt-get install -y libtbb2 
~~~

After that, I had started setting up the cmake build environment, but I was unable to do work out a "full build from github image" in the dockerfile.  What I started is still in the dockerfile, though. Perhaps someone more clever with cmake than I can propose a method.

After that, the rest of the dockerfile is pretty close to default with the exception of copying the nuget.config and ../nupkgs folder into the build image:
~~~dockerfile
WORKDIR /src
# copy any packages we've generated
COPY ["nupkgs/*.nupkg", "nupkgs/"]

# copy a nuget.config for testWebApi2 so it knows to look for our packages in the nupkgs folder
COPY ["testWebApi2/nuget.config", "testWebApi2/"]
~~~

## Summary
Hopefully, this will build for anyone wanting to play with cross platform C++ cmake in vs2022 with dotnet consumers.

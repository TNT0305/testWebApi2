#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# install runtime dependencies, (libtbb2 - intel threading building blocks)
RUN set -ex;         \
    apt-get update;  \
    apt-get install -y libtbb2 

###########################################Create a cmake build environment
# install latest cmake and tbb development package.
# Do not include native target in cmake-base.  We're trying to create a generic cmake build environment in its own image.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS cmake-base

# NOT BUILDING IN LINUX CONTAINER FOR NOW.  Would be nice to be able to, though.
#RUN set -ex; \
    #apt-get update;  \
    #apt-get install -y gpg wget libtbb-dev g++ gdb make ninja-build rsync zip;
#
#RUN set -ex; \
    #wget -O - https://apt.kitware.com/keys/kitware-archive-latest.asc 2>/dev/null | gpg --dearmor - | tee /usr/share/keyrings/kitware-archive-keyring.gpg >/dev/null; \
    #echo 'deb [signed-by=/usr/share/keyrings/kitware-archive-keyring.gpg] https://apt.kitware.com/ubuntu/ focal main' | tee /etc/apt/sources.list.d/kitware.list >/dev/null; \
    #apt-get update; \
    #rm /usr/share/keyrings/kitware-archive-keyring.gpg; \
    #apt-get install kitware-archive-keyring; \
    #apt-get update;  \
    #apt-get install -y cmake;
#
###########################################Add our native project to the cmake build environment image
FROM cmake-base as build
WORKDIR /src/native

#
#COPY ["testWebApi2/native/testStaticLib/*", "testStaticLib/"]
#COPY ["testWebApi2/native/testSharedLibrary/*", "testSharedLibrary/"]
#COPY ["testWebApi2/native/testCMake/*", "testCMake/"]
#COPY ["testWebApi2/native/CMakeLists.txt", "/src/native/"]
#COPY ["testWebApi2/native/CMakePresets.json", "/src/native/"]
#
#RUN cmake --preset linux-release 
#RUN cmake --build --preset linux-release # output in testCMake/out/build/linux-release
#RUN cmake --install out/build/linux-release/testSharedLibrary
#
#COPY ["testCMake/out/build/linux-release/testSharedLibrary/*.so", "/app/publish"]

WORKDIR /src
# copy any packages we've generated
COPY ["nupkgs/*.nupkg", "nupkgs/"]

# copy a nuget.config for testWebApi2 so it knows to look for our packages in the nupkgs folder
COPY ["testWebApi2/nuget.config", "testWebApi2/"]

COPY ["testWebApi2/testWebApi2.csproj", "testWebApi2/"]
RUN dotnet restore "testWebApi2/testWebApi2.csproj"
COPY . .
WORKDIR "/src/testWebApi2"
RUN dotnet build "testWebApi2.csproj" -c Release -o /app/build

###########################################Publish image into public folder in publish image (based on build image) -- unsure of the need for this intermediate image, but it was in there by default
FROM build AS publish
RUN dotnet publish "testWebApi2.csproj" -c Release -o /app/publish

###########################################Copy published output into /app directory of final image (based on aspnet:6.0 base image)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# COPY --from=publish /src/native/out/build/linux-release/testSharedLibrary/*.so .
ENTRYPOINT ["dotnet", "testWebApi2.dll"]
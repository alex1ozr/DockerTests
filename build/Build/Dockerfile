##################### Base stage #####################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080

RUN groupadd --gid 999 dotnet && \
    useradd --create-home --home-dir /app --uid 999 --gid dotnet dotnet

USER 999:999

##################### Build stage #####################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG Version=0.0.0.0
ARG AssemblyVersion=0.0.0.0
ARG FileVersion=0.0.0.0
ARG InformationalVersion=0.0.0.0

# Output version info
RUN echo Application Version: ${Version}
RUN echo Assembly Version: ${AssemblyVersion}
RUN echo File Version: ${FileVersion}
RUN echo Informational Version: ${InformationalVersion}

# Install the Aspire workload
RUN dotnet workload install aspire

WORKDIR /app/src
COPY . .

RUN dotnet restore
RUN dotnet build \
    --configuration Release \
    --no-restore \
    --nologo \
    -fl1 "/flp1:logFile=buildErrors.log;errorsonly" \
    -p:Version="${Version}" \
    -p:AssemblyVersion="${AssemblyVersion}" \
    -p:FileVersion="${FileVersion}" \
    -p:InformationalVersion="${InformationalVersion}" \
    || exit 0
 
# Making sure that the build errors are logged as errors
RUN sed -i -e 's/^/[build-error]/' buildErrors.log
RUN cat buildErrors.log
RUN test -s buildErrors.log && exit 1 || true

##################### Unit tests stage #####################
FROM build AS test

RUN dotnet test \
    --filter "Category!=IntegrationTests" \
    --configuration Release \
    --verbosity normal \
    --no-build \
    --no-restore \
    --nologo

##################### Publish artifacts stage #####################
FROM test AS publish

RUN dotnet pack \
    --configuration Release \
    --output "/app/artifacts/nuget" \
    --no-build \
    --no-restore \
    --nologo \
    -p:PackageVersion="${Version}"

RUN dotnet publish \
    ./src/Api/Api.csproj \
    --configuration Release \
    --output /app/publish \
    --no-build \
    --no-restore \
    --nologo

##################### Final stage #####################
FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /app/artifacts /app/artifacts

ENTRYPOINT ["dotnet", "DockerTestsSample.Api.dll"]

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
EXPOSE 8080

# Copy csproj and restore as distinct layers
COPY ./RITA.WebAPI/RITA.WebAPI.Api/*.csproj ./
COPY nuget.config ./
RUN curl -skL https://tools.circleci.foc.zone/install-certs | bash -
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish RITA.WebAPI.sln -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "RITA.WebAPI.Api.dll"]

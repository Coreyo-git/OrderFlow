FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY ["src/CustomerService/CustomerService.csproj", "src/CustomerService/"]
COPY ["src/CustomerService.Domain/CustomerService.Domain.csproj", "src/CustomerService.Domain/"]
COPY ["src/CustomerService.Application/CustomerService.Application.csproj", "src/CustomerService.Application/"]
COPY ["src/CustomerService.Infrastructure/CustomerService.Infrastructure.csproj", "src/CustomerService.Infrastructure/"]
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
RUN dotnet restore "src/CustomerService/CustomerService.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/CustomerService"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.dll"]
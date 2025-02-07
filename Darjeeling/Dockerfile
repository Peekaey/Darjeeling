# Use the runtime image for the final stage.
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Build stage using the SDK image.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Since the Dockerfile is in Darjeeling/Darjeeling, we can copy the project files directly.
COPY ["Darjeeling.csproj", "./"]

# Restore dependencies.
RUN dotnet restore "Darjeeling.csproj"

# Copy the rest of the source code.
COPY . .

# Build the project.
RUN dotnet build "Darjeeling.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Darjeeling.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image.
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Darjeeling.dll"]
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /source

# This is the architecture you’re building for.
ARG TARGETARCH

# --- OPTIMIZATION ---
# Copy only the project and solution files first. This allows Docker to cache the
# restored packages layer. The download will only re-run if these files change.
# IMPORTANT: This assumes your project is named 'user.csproj' and is in the root
# of the build context. Adjust the filename if it's different.
COPY *.sln .
COPY user.csproj .

# Restore dependencies for the specific project
RUN dotnet restore "user.csproj"

# Copy the rest of the source code
COPY . .

# Publish the application, specifying the project to avoid ambiguity
# This will create user.dll and its dependencies in the /app folder.
RUN dotnet publish "user.csproj" -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

################################################################################
# STAGE 2: Create the final, minimal runtime image

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app .

# Switch to a non-privileged user for security
USER $APP_UID

# --- FIX ---
# The entrypoint now correctly points to user.dll, which was created by the
# 'dotnet publish "user.csproj"' command above.
ENTRYPOINT ["dotnet", "user.dll"]

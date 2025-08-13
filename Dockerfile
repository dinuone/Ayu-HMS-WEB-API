
# --- build image ---
##alpine - smaller size image 5mb - 
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build 
WORKDIR /src
COPY . .
RUN dotnet publish "Harendra_Ayurveda_HMS_WEB_API.csproj"  -o /published /p:UseAppHost=false



# --- runtime image ---
 FROM  mcr.microsoft.com/dotnet/aspnet:8.0-alpine
 WORKDIR /app
 COPY --from=build /published .
 EXPOSE 5000
 ENTRYPOINT [ "dotnet", "Harendra_Ayurveda_HMS_WEB_API.dll" ] \
 # set the container startup command
 # when the container runs, its execute the dotnet Harendra_Ayurveda_HMS_WEB_API.dll









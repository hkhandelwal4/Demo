FROM gcr.io/google-appengine/aspnetcore:2.1
RUN mkdir /app
WORKDIR /app

COPY ProductAPI.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish "AmCart v1.0.sln" -c Release -o out

CMD ["dotnet", "out/ProductAPI.dll"]
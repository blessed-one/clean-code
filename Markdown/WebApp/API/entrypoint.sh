#!/bin/bash

# Ожидание доступности базы данных
echo "Waiting for PostgreSQL to start..."
while ! nc -z db 5432; do
  sleep 1
done
echo "PostgreSQL started"

# Ожидание доступности MinIO
echo "Waiting for MinIO to start..."
while ! nc -z minio 9000; do
  sleep 1
done
echo "MinIO started"

# Выполнение миграций
echo "Applying migrations..."
dotnet ef migrations add initial --startup-project API --project Persistence
dotnet ef database update --startup-project API --project Persistence

# Запуск основного приложения
echo "Starting API..."
exec dotnet API.dll
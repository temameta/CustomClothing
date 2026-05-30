# CustomClothing API — Сервис заказа кастомной одежды

Backend-приложение на базе **ASP.NET Core Minimal API** для управления процессом создания и продажи кастомной одежды. Проект реализует разделение на клиентскую и административную части, поддержку сложных бизнес-сценариев и интеграцию с PostgreSQL.

## Технологии

*   **Платформа:** .NET 10 (C# 13)
*   **API:** Minimal API с разделением на группы маршрутов
*   **База данных:** PostgreSQL
*   **ORM:** Entity Framework Core (Npgsql)
*   **Документация:** Swagger / OpenAPI с детальным описанием эндпоинтов

---

## Структура проекта

Проект организован по слоям для обеспечения чистоты кода:
*   `api/` — Статические классы с маршрутами (Endpoints).
*   `database/` — Контекст базы данных (`AppDbContext`).
*   `dto/` — Объекты передачи данных (разделены на `request` и `response`).
*   `interfaces/` — Интерфейсы сервисов (`IClothingService`, `IAdminService`).
*   `model/` — Доменные сущности базы данных.
*   `services/` — Бизнес-логика приложения.

---

## Сущности и связи

1.  **ClothingCategory (Категория):** Куртки, футболки, штаны.
2.  **CompletedWork (Портфолио):** Готовые работы мастера. Имеют флаг `IsPublic`.
3.  **DesignRequest (Заявка):** Индивидуальный запрос клиента. Связан с категорией. Имеет статусы: `Pending`, `Completed`.
4.  **Order (Заказ):** Факт продажи. Связан с `CompletedWork`. Имеет статусы: `New`, `Cancelled`, `Completed`.

---

## Эндпоинты API

### Общие
*   `GET /` — Приветственная страница с информацией об API.

### Customer (Заказчик) — `/api`
*   `GET /api/catalog` — Просмотр публичного портфолио работ.
*   `GET /api/categories` — Список категорий одежды.
*   `POST /api/requests` — Создание новой заявки на индивидуальный дизайн.
*   `POST /api/admin/requests/{id}/finalize` — Завершение работы по заявке: создание работы в портфолио и автоматическое оформление заказа.
*   `POST /api/orders` — Заказ существующей вещи из каталога.
*   `POST /api/orders/{id}/cancel` — **[Бизнес-логика]** Отмена заказа клиентом (только если статус `New`).

### Admin (Управление) — `/api/admin`
*   **Заявки:**
    *   `GET /api/admin/requests` — Список всех заявок.
    *   `GET /api/admin/requests/{id}` — Детальная информация о заявке.
    *   `DELETE /api/admin/requests/{id}` — Удаление заявки.
*   **Заказы:**
    *   `GET /api/admin/orders` — Просмотр всех заказов со статусами.
    *   `GET /api/admin/orders/{id}` — Детали конкретного заказа.
    *   `DELETE /api/admin/orders/{id}` — Удаление записи заказа.

---

## Ключевые бизнес-сценарии

1.  **Сквозная операция "Финализация":** При вызове менеджером эндпоинта `finalize`, система одновременно обновляет статус заявки, переносит её описание в каталог выполненных работ и генерирует запись в таблице заказов.
2.  **Валидация отмены:** Пользователь не может отменить заказ, если менеджер уже перевел его в статус `Completed`. В таком случае API вернет ошибку `400 Bad Request`.

---

## Запуск и настройка

### 1. Инфраструктура (Docker)
Запустите PostgreSQL:
```bash
docker run --name clothing-postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=ClothingDb -p 5432:5432 -d postgres:latest
```

### 2. Конфигурация
В `appsettings.json` проверьте строку подключения:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=ClothingDb;Username=postgres;Password=postgres"
}
```

### 3. База данных (Миграции)
```bash
dotnet ef database update
```

### 4. Запуск
```bash
dotnet run
```
Swagger-документация: `http://localhost:5000/swagger`

---

## Обработка ответов
*   `200 OK` — Успешное выполнение.
*   `201 Created` — Объект (заявка/заказ) успешно создан.
*   `204 No Content` — Успешное удаление.
*   `400 Bad Request` — Нарушение бизнес-логики (содержит JSON с описанием ошибки).
*   `404 Not Found` — Ресурс не найден.
-- Verifica se já existem usuários
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "users" LIMIT 1) THEN
        -- Insere usuários apenas se não existirem
        INSERT INTO "users" ("Id", "Username", "UserEmail", "UserPassword", "Role", "CreatedAt", "UpdatedAt")
        VALUES 
            (gen_random_uuid(), 'admin', 'admin@bimworkplace.com', '$2b$10$6WwHqTup1wC0f9y7T5FfS.O8PW7uH3Zf9R3V3b0e6zVQq6P0vFJ5i', 0, NOW(), NOW()),
            (gen_random_uuid(), 'manager', 'manager@bimworkplace.com', '$2b$10$KxE9bF2uPqX8mC4gT1ZsUuT3nL6vD8yQ7hJ2kW5rE9cB1pN4sY0a', 1, NOW(), NOW()),
            (gen_random_uuid(), 'member', 'member@bimworkplace.com', '$2b$10$DzQ8nL2sR5vF1cG7hJ3kM9xT4bV6pN2wY8zC0dS3uE5rH9jL1qK', 2, NOW(), NOW());
    END IF;
END $$;
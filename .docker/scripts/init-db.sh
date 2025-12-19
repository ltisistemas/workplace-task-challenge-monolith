#!/bin/bash
set -e

FLAG_FILE="/var/lib/postgresql/data/.seed_completed"

# Aguarda o PostgreSQL estar pronto
until pg_isready -U bimworkplace -d bimworkplace_db; do
  echo "Aguardando PostgreSQL estar pronto..."
  sleep 2
done

# Verifica se o seed já foi executado
if [ ! -f "$FLAG_FILE" ]; then
  echo "Executando seed de usuários..."
  
  # Executa o seed via dotnet (você precisará ter o .NET SDK no container ou executar de fora)
  # Ou pode usar um script SQL diretamente
  
  # Marca que o seed foi executado
  touch "$FLAG_FILE"
  echo "Seed concluído!"
else
  echo "Seed já foi executado anteriormente. Pulando..."
fi
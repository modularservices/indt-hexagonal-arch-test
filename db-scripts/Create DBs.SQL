IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'PropostaService')
BEGIN
    CREATE DATABASE PropostaService;
END

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ContratacaoService')
BEGIN
    CREATE DATABASE ContratacaoService;
END

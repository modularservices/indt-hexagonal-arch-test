namespace Indt.Teste.PropostaService.Api.Setup;

public static class DatabaseScripts
{
    public const string CreateDB =
        @"

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'PropostaService#Env')
BEGIN
    CREATE DATABASE PropostaService#Env;
END

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ContratacaoService#Env')
BEGIN
    CREATE DATABASE ContratacaoService#Env;
END
            ";

    public const string Setup =
        @"

USE PropostaService#Env;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Cliente')
BEGIN
    CREATE TABLE Cliente (
        Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Nome            VARCHAR(150)     NOT NULL

        CONSTRAINT PK_Cliente PRIMARY KEY (Id)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Seguradora')
BEGIN
    CREATE TABLE Seguradora (
        Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Nome            VARCHAR(150)     NOT NULL

        CONSTRAINT PK_Seguradora PRIMARY KEY (Id)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Corretor')
BEGIN
    CREATE TABLE Corretor (
        Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Nome            VARCHAR(150)     NOT NULL

        CONSTRAINT PK_Corretor PRIMARY KEY (Id)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProdutoSeguro')
BEGIN
    CREATE TABLE ProdutoSeguro (
        Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Nome            VARCHAR(100)     NOT NULL, -- ex: ""Seguro Auto""

        CONSTRAINT PK_ProdutoSeguro PRIMARY KEY (Id)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Proposta')
BEGIN
    CREATE TABLE Proposta (
        Id                  UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        NumeroProposta      VARCHAR(30)      NOT NULL,
        ClienteId           UNIQUEIDENTIFIER NOT NULL,
        SeguradoraId        UNIQUEIDENTIFIER NOT NULL,
        CorretorId          UNIQUEIDENTIFIER NOT NULL,
        ProdutoSeguroId     UNIQUEIDENTIFIER NOT NULL,
        Valor               DECIMAL(18, 2)   NOT NULL,
        Status              TINYINT          NOT NULL DEFAULT 1, -- 1 = EmAnalise, 2 = Aprovada, 3 = Rejeitada
        DataCriacao         DATETIME2        NULL,

        CONSTRAINT PK_Proposta PRIMARY KEY (Id),
        CONSTRAINT UQ_Proposta_NumeroProposta UNIQUE (NumeroProposta),

        CONSTRAINT FK_Proposta_Cliente
            FOREIGN KEY (ClienteId) REFERENCES Cliente (Id),
        CONSTRAINT FK_Proposta_Seguradora
            FOREIGN KEY (SeguradoraId) REFERENCES Seguradora (Id),
        CONSTRAINT FK_Proposta_Corretor
            FOREIGN KEY (CorretorId) REFERENCES Corretor (Id),
        CONSTRAINT FK_Proposta_ProdutoSeguro
            FOREIGN KEY (ProdutoSeguroId) REFERENCES ProdutoSeguro (Id),

        -- Garante que o status fique restrito ao que o desafio define
        -- 1 = EmAnalise | 2 = Aprovada | 3 = Rejeitada | 4 = Contratada
        CONSTRAINT CK_Proposta_Status
            CHECK (Status IN (1, 2, 3, 4)),

        -- Valor da proposta nao pode ser negativo
        CONSTRAINT CK_Proposta_Valor
            CHECK (Valor >= 0)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FirstTime')
BEGIN
    CREATE TABLE FirstTime (
    Id INT
    );

    INSERT INTO Cliente (Id, Nome)
    VALUES (
        '9A0F7E5B-1B74-4C52-8F0D-8A5C73A8B5D1',
        'João da Silva'
    );

    INSERT INTO Seguradora (Id, Nome)
    VALUES (
        'A1B2C3D4-E5F6-7890-ABCD-EF1234567890',
        'Porto Seguro');

    INSERT INTO Corretor (Id, Nome)
    VALUES (
        'A1B2C3D4-E5F6-47A8-9B0C-123456789ABC',
        'Corretora Alpha'
    );

    INSERT INTO ProdutoSeguro (Id, Nome)
    VALUES (
        'F0E1D2C3-B4A5-4678-9012-ABCDEF123456',
        'Seguro Residencial Premium'
    );
END

USE ContratacaoService#Env;

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Contratacao')
BEGIN
    CREATE TABLE Contratacao
    (
        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID()
        CONSTRAINT PK_Contratacao PRIMARY KEY (Id),

        PropostaId UNIQUEIDENTIFIER NOT NULL,

        DataContratacao DATETIME2 NOT NULL
    );
END;
            ";
}

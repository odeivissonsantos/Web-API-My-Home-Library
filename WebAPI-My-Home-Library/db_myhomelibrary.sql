USE db_myhomelibrary
GO

CREATE TABLE [usuario] (
	ide_usuario bigint IDENTITY(1,1) NOT NULL,
	nome varchar(25) NOT NULL,
	sobrenome varchar(60) NOT NULL,
	email varchar(150) NOT NULL,
	senha varchar(max) NOT NULL,
	dtc_inclusao datetime NOT NULL,
	sts_exclusao bit NOT NULL,
	ide_perfil int NOT NULL,
  CONSTRAINT [PK_USUARIO] PRIMARY KEY CLUSTERED
  (
  [ide_usuario] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)
)
GO

CREATE TABLE [perfil] (
	ide_perfil int IDENTITY(1,1) NOT NULL,
	nome varchar(25) NOT NULL,
	descricao varchar(150) NOT NULL,
	sts_exclusao bit NOT NULL,
  CONSTRAINT [PK_PERFIL] PRIMARY KEY CLUSTERED
  (
  [ide_perfil] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO

CREATE TABLE [livro] (
	ide_livro bigint IDENTITY(1,1) NOT NULL,
	autor varchar(100) NOT NULL,
	ano int NOT NULL,
	editora varchar(50) NOT NULL,
	codigo_barras bigint  NULL,
	url_capa varchar(max) NULL,
	titulo varchar(100) NOT NULL,
	observacao varchar(500) NULL,
  CONSTRAINT [PK_LIVRO] PRIMARY KEY CLUSTERED
  (
  ide_livro ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO

CREATE TABLE [usuario_livro] (
	ide_usuario_livro bigint IDENTITY(1,1) NOT NULL,
	ide_usuario bigint NOT NULL,
	ide_livro bigint NOT NULL,
  CONSTRAINT [PK_USUARIO_LIVRO] PRIMARY KEY CLUSTERED
  (
  ide_usuario_livro ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO

CREATE TABLE [usuario_token] (
	ide_usuario_token bigint IDENTITY(1,1) NOT NULL,
	ide_usuario bigint NOT NULL,
	token varchar(max) NULL,
	dtc_exp_token datetime NULL,
  CONSTRAINT [PK_USUARIO_TOKEN] PRIMARY KEY CLUSTERED
  (
  ide_usuario_token ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO

CREATE TABLE [usuario_acessos] (
	ide_usuario_acessos bigint IDENTITY(1,1) NOT NULL,
	ide_usuario bigint NOT NULL,
	qtd_acessos bigint NULL,
	dtc_ultimo_acesso datetime NULL,
  CONSTRAINT [PK_USUARIO_ACESSOS] PRIMARY KEY CLUSTERED
  (
  ide_usuario_acessos ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO

ALTER TABLE [usuario] WITH CHECK ADD CONSTRAINT [usuario_fk0] FOREIGN KEY ([ide_perfil]) REFERENCES [perfil]([ide_perfil])
ON UPDATE CASCADE
GO

ALTER TABLE [usuario] CHECK CONSTRAINT [usuario_fk0]
GO

INSERT INTO perfil(nome, descricao, sts_exclusao)
VALUES
('Administrador', 'Administrador', 0),
('Padrão', 'Usuário Padrão', 0)

INSERT INTO usuario(nome, sobrenome, email, senha, dtc_inclusao, sts_exclusao, ide_perfil)
VALUES
('Deivisson', 'Santos', 'deivisson.santos@detran.ba.gov.br', 'ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413',  GETDATE(), 0, 1)

INSERT INTO usuario_token(ide_usuario)
VALUES
(1)

INSERT INTO usuario_acessos(ide_usuario, qtd_acessos)
VALUES
(1, 0)

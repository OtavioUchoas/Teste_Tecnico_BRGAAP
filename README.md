# 🧩 Teste Técnico — BRGAAP

## 📘 Resumo da Aplicação
Aplicação desenvolvida em **C#** e **JavaScript** que consome dados de uma **API pública** e os apresenta ao usuário por meio do **framework SAPUI5**.

---

## ⚙️ Funcionalidades
- [ ] **Listagem de Tarefas** — Exibe tarefas em tempo real divididas em: *id*, *userId*, *title* e *completed*, com opção de visualizar detalhes.  
- [ ] **Características da Aplicação** — Inclui campo de pesquisa com *debounce*, filtro por título e status em tempo real, além de navegação utilizando *Router*.  
- [ ] **Sincronização de Dados Externos** — Consome dados de uma API pública e os salva no banco de dados local.  

---

## ⚠️ Avisos
> ⚠️ **Antes de realizar qualquer consulta ou teste, execute o comando de sincronização (Sync).**

**Acesse a aplicação pelos seguintes endereços locais:**
- 🔗 https://localhost:7075/webapp/index.html
- 🔗 http://localhost:5224/webapp/index.html
- Ou .../swagger/index.html para acesso ao swagger

---

## 🧰 Tecnologias Usadas
- **Linguagens:** C#, JavaScript  
- **Frameworks:** SAPUI5, Entity Framework Core  
- **Banco de Dados:** SQL Server  
- **Testes:** xUnit  

---

## 🚀 Como Executar
```bash
# Clonar o repositório
git clone https://github.com/OtavioUchoas/Teste_Tecnico_BRGAAP.git

# Entrar na pasta do projeto
cd Teste_Tecnico_BRGAAP

# Limpar e compilar a solução
dotnet clean
dotnet build

# Entrar na pasta API
cd ListagemTarefa.API

# Executar a aplicação
dotnet run

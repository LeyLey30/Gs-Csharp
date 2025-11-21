HybridWorkApp - Aplicativo WPF para conciliar vida pessoal e profissional (regimes híbridos)
==================================================================================================

O projeto foi gerado automaticamente para Visual Studio 2022 (.NET 6 WPF). Ele contém:

- Demonstração de POO com herança e polimorfismo (User / Employee).
- Interface WPF com duas telas (Dashboard e Cronograma).
- Lógica de negócio (ScheduleService) que calcula um índice de equilíbrio (simples simulação).
- Persistência simples em JSON (salva em Meus Documentos/HybridWorkApp/userdata.json).
- Exportação CSV do cronograma para Desktop.
- Instruções básicas abaixo.

Como abrir no Visual Studio 2022:
1. Extraia o conteúdo do arquivo HybridWorkApp.zip em uma pasta.
2. Abra o arquivo HybridWorkApp.sln no Visual Studio 2022.
3. Certifique-se de ter .NET 6 SDK instalado. (Visual Studio 2022 Preview/17.3+ suporta .NET 6 WPF).
4. Build -> Run (F5).

Estrutura principal:
- HybridWorkApp/HybridWorkApp.csproj
- HybridWorkApp/App.xaml, App.xaml.cs
- HybridWorkApp/MainWindow.xaml, MainWindow.xaml.cs
- HybridWorkApp/Models (ScheduleItem, User/Employee/UserData)
- HybridWorkApp/Services (ScheduleService, PersistenceService)

Observações:
- O código é um ponto de partida completo e comentado; sinta-se à vontade para estender com autenticação,
  integração com calendários (Google/Microsoft), notificações, UI/UX mais rica e testes unitários.

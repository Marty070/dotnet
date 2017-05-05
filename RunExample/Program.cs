using System.IO;
using System.Linq;
using Structurizr;
using Structurizr.Client;
using Structurizr.Util;

namespace RunExample
{
    class Program
    {
        private const string AlertTag = "Alert";

        static void Main(string[] args)
        {
            //CreateFinancialExample();

            //CreateDemoExample();
            CreateWidgetsExample();
            //GetDemoExample();
        }

        private static void GetDemoExample()
        {
            StructurizrClient structurizrClient = new StructurizrClient("cd952380-622e-441a-a330-068bf547142b", "1aedc2cd-5ae1-402e-a72c-9688a89c15ed");
            var workSpace = structurizrClient.GetWorkspace(31931);

        }


        private static void CreateWidgetsExample()
        {
            string ExternalPersonTag = "External Person";
            string ExternalSoftwareSystemTag = "External Software System";

            string InternalPersonTag = "Internal Person";
            string InternalSoftwareSystemTag = "Internal Software System";


            Workspace workspace = new Workspace("Widgets Limited", "Sells widgets to customers online.");
            Model model = workspace.Model;
            ViewSet views = workspace.Views;
            Styles styles = views.Configuration.Styles;

            model.Enterprise = new Enterprise("Widgets Limited");

            Person customer = model.AddPerson(Location.External, "Customer", "A customer of Widgets Limited...");
            Person customerServiceUser = model.AddPerson(Location.Internal, "Customer Service Agent", "Deals with customer enquiries.");
            SoftwareSystem ecommerceSystem = model.AddSoftwareSystem(Location.Internal, "E-commerce System", "Allows customers to buy widgets online via the widgets.com website.");
            SoftwareSystem fulfilmentSystem = model.AddSoftwareSystem(Location.Internal, "Fulfilment System", "Responsible for processing and shipping of customer orders.");
            SoftwareSystem taxamo = model.AddSoftwareSystem(Location.External, "Taxamo", "Calculates local tax (for EU B2C customers) and acts as a front-end for Braintree Payments.");
            taxamo.Url = "https://www.taxamo.com";
            SoftwareSystem braintreePayments = model.AddSoftwareSystem(Location.External, "Braintree Payments", "Processes credit card payments on behalf of Widgets Limited.");
            braintreePayments.Url = "https://www.braintreepayments.com";
            SoftwareSystem jerseyPost = model.AddSoftwareSystem(Location.External, "Jersey Post", "Calculates worldwide shipping costs for packages.");

            model.People.Where(p => p.Location == Location.External).ToList().ForEach(p => p.AddTags(ExternalPersonTag));
            model.People.Where(p => p.Location == Location.Internal).ToList().ForEach(p => p.AddTags(InternalPersonTag));

            model.SoftwareSystems.Where(ss => ss.Location == Location.External).ToList().ForEach(ss => ss.AddTags(ExternalSoftwareSystemTag));
            model.SoftwareSystems.Where(ss => ss.Location == Location.Internal).ToList().ForEach(ss => ss.AddTags(InternalSoftwareSystemTag));

            customer.InteractsWith(customerServiceUser, "Asks questions to", "Telephone");
            customerServiceUser.Uses(ecommerceSystem, "Looks up order information using");
            customer.Uses(ecommerceSystem, "Places orders for widgets using");
            ecommerceSystem.Uses(fulfilmentSystem, "Sends order information to");
            fulfilmentSystem.Uses(jerseyPost, "Gets shipping charges from");
            ecommerceSystem.Uses(taxamo, "Delegates credit card processing to");
            taxamo.Uses(braintreePayments, "Uses for credit card processing");

            EnterpriseContextView enterpriseContextView = views.CreateEnterpriseContextView("enterpriseContext", "The enterprise context for Widgets Limited.");
            enterpriseContextView.AddAllElements();

            SystemContextView systemContextView = views.CreateSystemContextView(ecommerceSystem, "systemContext", "A description of the Widgets Limited e-commerce system.");
            systemContextView.AddNearestNeighbours(ecommerceSystem);
            systemContextView.Remove(customer.GetEfferentRelationshipWith(customerServiceUser));

            SystemContextView fulfilmentSystemContext = views.CreateSystemContextView(fulfilmentSystem, "FulfilmentSystemContext", "The system context diagram for the Widgets Limited fulfilment system.");
            fulfilmentSystemContext.AddNearestNeighbours(fulfilmentSystem);

            DynamicView dynamicView = views.CreateDynamicView("CustomerSupportCall", "A high-level overview of the customer support call process.");
            dynamicView.Add(customer, customerServiceUser);
            dynamicView.Add(customerServiceUser, ecommerceSystem);

            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle(Tags.Person) { Shape = Shape.Person });

            styles.Add(new ElementStyle(Tags.Element) { Color = "#ffffff" });
            styles.Add(new ElementStyle(ExternalPersonTag) { Background = "#EC5381" });
            styles.Add(new ElementStyle(ExternalSoftwareSystemTag) { Background = "#EC5381" });

            styles.Add(new ElementStyle(InternalPersonTag) { Background = "#B60037" });
            styles.Add(new ElementStyle(InternalSoftwareSystemTag) { Background = "#B60037" });

            StructurizrClient structurizrClient = new StructurizrClient("ac21b5ed-b3b1-429e-8bf4-d0e854bb2da8", "427cdee8-15f8-42e0-94c9-f80b37bd471c");
            structurizrClient.PutWorkspace(31961, workspace);

        }

        private static void CreateDemoExample()
        {


            Workspace workspace = new Workspace("DutchWorkz Model", "This is a model of my software system.");
            Model model = workspace.Model;

            Person user = model.AddPerson("User", "A user of my software system.");
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");
            user.Uses(softwareSystem, "Uses");

            ViewSet viewSet = workspace.Views;
            SystemContextView contextView = viewSet.CreateSystemContextView(softwareSystem, "context", "A simple example of a System Context diagram.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });

            StructurizrClient structurizrClient = new StructurizrClient("cd952380-622e-441a-a330-068bf547142b", "1aedc2cd-5ae1-402e-a72c-9688a89c15ed");
            structurizrClient.PutWorkspace(31931, workspace);
        }


        private static void CreateFinancialExample()
        {
            Workspace workspace = new Workspace("Financial Risk System",
                "A simple example C4 model based upon the financial risk system architecture kata, created using Structurizr for .NET");
            Model model = workspace.Model;

            // create the basic model
            SoftwareSystem financialRiskSystem = model.AddSoftwareSystem(Location.Internal, "Financial Risk System",
                "Calculates the bank's exposure to risk for product X");

            Person businessUser = model.AddPerson(Location.Internal, "Business User", "A regular business user");
            businessUser.Uses(financialRiskSystem, "Views reports using");

            Person configurationUser = model.AddPerson(Location.Internal, "Configuration User",
                "A regular business user who can also configure the parameters used in the risk calculations");
            configurationUser.Uses(financialRiskSystem, "Configures parameters using");

            SoftwareSystem tradeDataSystem = model.AddSoftwareSystem(Location.Internal, "Trade Data System",
                "The system of record for trades of type X");
            financialRiskSystem.Uses(tradeDataSystem, "Gets trade data from");

            SoftwareSystem referenceDataSystem = model.AddSoftwareSystem(Location.Internal, "Reference Data System",
                "Manages reference data for all counterparties the bank interacts with");
            financialRiskSystem.Uses(referenceDataSystem, "Gets counterparty data from");

            SoftwareSystem emailSystem = model.AddSoftwareSystem(Location.Internal, "E-mail system", "Microsoft Exchange");
            financialRiskSystem.Uses(emailSystem, "Sends a notification that a report is ready to");
            emailSystem.Delivers(businessUser, "Sends a notification that a report is ready to", "E-mail message",
                InteractionStyle.Asynchronous);

            SoftwareSystem centralMonitoringService = model.AddSoftwareSystem(Location.Internal, "Central Monitoring Service",
                "The bank-wide monitoring and alerting dashboard");
            financialRiskSystem.Uses(centralMonitoringService, "Sends critical failure alerts to", "SNMP",
                InteractionStyle.Asynchronous).AddTags(AlertTag);

            SoftwareSystem activeDirectory = model.AddSoftwareSystem(Location.Internal, "Active Directory",
                "Manages users and security roles across the bank");
            financialRiskSystem.Uses(activeDirectory, "Uses for authentication and authorisation");

            Container webApplication = financialRiskSystem.AddContainer("Web Application",
                "Allows users to view reports and modify risk calculation parameters", "ASP.NET MVC");
            businessUser.Uses(webApplication, "Views reports using");
            configurationUser.Uses(webApplication, "Modifies risk calculation parameters using");
            webApplication.Uses(activeDirectory, "Uses for authentication and authorisation");

            Container batchProcess = financialRiskSystem.AddContainer("Batch Process", "Calculates the risk", "Windows Service");
            batchProcess.Uses(emailSystem, "Sends a notification that a report is ready to");
            batchProcess.Uses(tradeDataSystem, "Gets trade data from");
            batchProcess.Uses(referenceDataSystem, "Gets counterparty data from");
            batchProcess.Uses(centralMonitoringService, "Sends critical failure alerts to", "SNMP",
                InteractionStyle.Asynchronous).AddTags(AlertTag);

            Container fileSystem = financialRiskSystem.AddContainer("File System", "Stores risk reports", "Network File Share");
            webApplication.Uses(fileSystem, "Consumes risk reports from");
            batchProcess.Uses(fileSystem, "Publishes risk reports to");

            Component scheduler = batchProcess.AddComponent("Scheduler",
                "Starts the risk calculation process at 5pm New York time", "Quartz.NET");
            Component orchestrator = batchProcess.AddComponent("Orchestrator", "Orchestrates the risk calculation process", "C#");
            Component tradeDataImporter = batchProcess.AddComponent("Trade data importer",
                "Imports data from the Trade Data System", "C#");
            Component referenceDataImporter = batchProcess.AddComponent("Reference data importer",
                "Imports data from the Reference Data System", "C#");
            Component riskCalculator = batchProcess.AddComponent("Risk calculator", "Calculates risk", "C#");
            Component reportGenerator = batchProcess.AddComponent("Report generator",
                "Generates a Microsoft Excel compatible risk report", "C# and Microsoft.Office.Interop.Excel");
            Component reportPublisher = batchProcess.AddComponent("Report distributor",
                "Publishes the report to the web application", "C#");
            Component emailComponent = batchProcess.AddComponent("E-mail component", "Sends e-mails", "C#");
            Component reportChecker = batchProcess.AddComponent("Report checker",
                "Checks that the report has been generated by 9am singapore time", "C#");
            Component alertComponent = batchProcess.AddComponent("Alert component", "Sends SNMP alerts", "C# and #SNMP Library");

            scheduler.Uses(orchestrator, "Starts");
            scheduler.Uses(reportChecker, "Starts");
            orchestrator.Uses(tradeDataImporter, "Imports data using");
            tradeDataImporter.Uses(tradeDataSystem, "Imports data from");
            orchestrator.Uses(referenceDataImporter, "Imports data using");
            referenceDataImporter.Uses(referenceDataSystem, "Imports data from");
            orchestrator.Uses(riskCalculator, "Calculates the risk using");
            orchestrator.Uses(reportGenerator, "Generates the risk report using");
            orchestrator.Uses(reportPublisher, "Publishes the risk report using");
            reportPublisher.Uses(fileSystem, "Publishes the risk report to");
            orchestrator.Uses(emailComponent, "Sends e-mail using");
            emailComponent.Uses(emailSystem, "Sends a notification that a report is ready to");
            reportChecker.Uses(alertComponent, "Sends alerts using");
            alertComponent.Uses(centralMonitoringService, "Sends alerts using", "SNMP", InteractionStyle.Asynchronous)
                .AddTags(AlertTag);

            // create some views
            ViewSet viewSet = workspace.Views;
            SystemContextView contextView = viewSet.CreateSystemContextView(financialRiskSystem, "Context", "");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            ContainerView containerView = viewSet.CreateContainerView(financialRiskSystem, "Containers", "");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();

            ComponentView componentViewForBatchProcess = viewSet.CreateComponentView(batchProcess, "Components", "");
            contextView.PaperSize = PaperSize.A3_Landscape;
            componentViewForBatchProcess.AddAllElements();
            componentViewForBatchProcess.Remove(configurationUser);
            componentViewForBatchProcess.Remove(webApplication);
            componentViewForBatchProcess.Remove(activeDirectory);

            // tag and style some elements
            Styles styles = viewSet.Configuration.Styles;
            financialRiskSystem.AddTags("Risk System");

            styles.Add(new ElementStyle(Tags.Element) { Color = "#ffffff", FontSize = 34 });
            styles.Add(new ElementStyle("Risk System") { Background = "#8a458a" });
            styles.Add(new ElementStyle(Tags.SoftwareSystem)
            {
                Width = 650,
                Height = 400,
                Background = "#510d51",
                Shape = Shape.Box
            });
            styles.Add(new ElementStyle(Tags.Person) { Width = 550, Background = "#62256e", Shape = Shape.Person });
            styles.Add(new ElementStyle(Tags.Container) { Width = 650, Height = 400, Background = "#a46ba4", Shape = Shape.Box });
            styles.Add(new ElementStyle(Tags.Component) { Width = 550, Background = "#c9a1c9", Shape = Shape.Box });

            styles.Add(new RelationshipStyle(Tags.Relationship) { Thickness = 4, Dashed = false, FontSize = 32, Width = 400 });
            styles.Add(new RelationshipStyle(Tags.Synchronous) { Dashed = false });
            styles.Add(new RelationshipStyle(Tags.Asynchronous) { Dashed = true });
            styles.Add(new RelationshipStyle(AlertTag) { Color = "#ff0000" });

            Documentation documentation = workspace.Documentation;
            FileInfo documentationRoot = new FileInfo(@"..\..\FinancialRiskSystem");
            documentation.Add(financialRiskSystem, SectionType.Context, DocumentationFormat.Markdown,
                new FileInfo(Path.Combine(documentationRoot.FullName, "context.md")));
            documentation.Add(financialRiskSystem, SectionType.FunctionalOverview, DocumentationFormat.Markdown,
                new FileInfo(Path.Combine(documentationRoot.FullName, "functional-overview.md")));
            documentation.Add(financialRiskSystem, SectionType.QualityAttributes, DocumentationFormat.Markdown,
                new FileInfo(Path.Combine(documentationRoot.FullName, "quality-attributes.md")));
            documentation.AddImages(documentationRoot);

            // add some example corporate branding
            Branding branding = viewSet.Configuration.Branding;
            branding.Font = new Font("Trebuchet MS");
            branding.Color1 = new ColorPair("#510d51", "#ffffff");
            branding.Color2 = new ColorPair("#62256e", "#ffffff");
            branding.Color3 = new ColorPair("#a46ba4", "#ffffff");
            branding.Color4 = new ColorPair("#c9a1c9", "#ffffff");
            branding.Color5 = new ColorPair("#c9a1c9", "#ffffff");
            branding.Logo =
                ImageUtils.GetImageAsDataUri(new FileInfo(Path.Combine(documentationRoot.FullName, "codingthearchitecture.png")));

            // and upload the model to structurizr.com
            StructurizrClient structurizrClient = new StructurizrClient("bd4ed4f5-c8f1-492d-8c35-28adebe2c325",
                "b0f29a7e-0d46-48ff-a728-6859978aa384");
            structurizrClient.PutWorkspace(31921, workspace);
        }
    }
}

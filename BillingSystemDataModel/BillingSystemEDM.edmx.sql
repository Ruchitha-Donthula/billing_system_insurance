
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/10/2024 02:54:10
-- Generated from EDMX file: C:\Users\Admin\source\repos\BillingSystem\BillingSystemDataModel\BillingSystemEDM.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BillingSystem];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BillAccountBillAccountPolicy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BillAccountPolicies] DROP CONSTRAINT [FK_BillAccountBillAccountPolicy];
GO
IF OBJECT_ID(N'[dbo].[FK_BillAccountBillingTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BillingTransactions] DROP CONSTRAINT [FK_BillAccountBillingTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_BillAccountInstallmentSummary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InstallmentSummaries] DROP CONSTRAINT [FK_BillAccountInstallmentSummary];
GO
IF OBJECT_ID(N'[dbo].[FK_BillAccountPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payments] DROP CONSTRAINT [FK_BillAccountPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_InstallmentSummaryInstallment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Installments] DROP CONSTRAINT [FK_InstallmentSummaryInstallment];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[BillAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BillAccounts];
GO
IF OBJECT_ID(N'[dbo].[BillAccountPolicies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BillAccountPolicies];
GO
IF OBJECT_ID(N'[dbo].[BillingTransactions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BillingTransactions];
GO
IF OBJECT_ID(N'[dbo].[InstallmentSummaries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InstallmentSummaries];
GO
IF OBJECT_ID(N'[dbo].[Installments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Installments];
GO
IF OBJECT_ID(N'[dbo].[Invoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Invoices];
GO
IF OBJECT_ID(N'[dbo].[InvoiceInstallments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceInstallments];
GO
IF OBJECT_ID(N'[dbo].[Payments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payments];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'BillAccounts'
CREATE TABLE [dbo].[BillAccounts] (
    [BillAccountId] int IDENTITY(1,1) NOT NULL,
    [BillAccountNumber] nvarchar(max)  NOT NULL,
    [BillingType] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [PayorName] nvarchar(max)  NOT NULL,
    [PayorAddress] nvarchar(max)  NOT NULL,
    [PaymentMethod] nvarchar(max)  NOT NULL,
    [DueDay] int  NOT NULL,
    [AccountTotal] float  NULL,
    [AccountPaid] float  NULL,
    [AccountBalance] float  NULL,
    [LastPaymentDate] datetime  NULL,
    [LastPaymentAmount] float  NULL,
    [PastDue] float  NULL,
    [FutureDue] float  NULL
);
GO

-- Creating table 'BillAccountPolicies'
CREATE TABLE [dbo].[BillAccountPolicies] (
    [BillAccountPolicyId] int IDENTITY(1,1) NOT NULL,
    [PolicyNumber] nvarchar(max)  NOT NULL,
    [BillAccountId] int  NOT NULL,
    [PayPlan] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'BillingTransactions'
CREATE TABLE [dbo].[BillingTransactions] (
    [BillingTransactionId] int IDENTITY(1,1) NOT NULL,
    [ActivityDate] datetime  NOT NULL,
    [TransactionType] nvarchar(max)  NOT NULL,
    [TransactionAmount] float  NULL,
    [InvoiceId] int  NULL,
    [PaymentId] int  NULL,
    [BillAccountId] int  NOT NULL
);
GO

-- Creating table 'InstallmentSummaries'
CREATE TABLE [dbo].[InstallmentSummaries] (
    [InstallmentSummaryId] int IDENTITY(1,1) NOT NULL,
    [PolicyNumber] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [BillAccountId] int  NOT NULL
);
GO

-- Creating table 'Installments'
CREATE TABLE [dbo].[Installments] (
    [InstallmentId] int IDENTITY(1,1) NOT NULL,
    [InstallmentSequenceNumber] int  NOT NULL,
    [InstallmentSendDate] datetime  NOT NULL,
    [InstallmentDueDate] datetime  NOT NULL,
    [DueAmount] float  NOT NULL,
    [PaidAmount] float  NULL,
    [BalanceAmount] float  NULL,
    [InvoiceStatus] nvarchar(max)  NULL,
    [InstallmentSummaryId] int  NOT NULL
);
GO

-- Creating table 'Invoices'
CREATE TABLE [dbo].[Invoices] (
    [InvoiceId] int IDENTITY(1,1) NOT NULL,
    [InvoiceNumber] nvarchar(max)  NOT NULL,
    [InvoiceDate] datetime  NOT NULL,
    [SendDate] datetime  NOT NULL,
    [BillAccountId] int  NOT NULL,
    [Status] nvarchar(max)  NULL,
    [InvoiceAmount] float  NOT NULL
);
GO

-- Creating table 'InvoiceInstallments'
CREATE TABLE [dbo].[InvoiceInstallments] (
    [InvoiceInstallmentId] int IDENTITY(1,1) NOT NULL,
    [InvoiceId] int  NOT NULL,
    [InstallmentId] int  NOT NULL
);
GO

-- Creating table 'Payments'
CREATE TABLE [dbo].[Payments] (
    [PaymentId] int IDENTITY(1,1) NOT NULL,
    [PaymentMethod] nvarchar(max)  NOT NULL,
    [PaymentFrom] nvarchar(max)  NOT NULL,
    [Amount] float  NOT NULL,
    [BillAccountNumber] nvarchar(max)  NOT NULL,
    [PaymentDate] datetime  NOT NULL,
    [InvoiceNumber] nvarchar(max)  NOT NULL,
    [PaymentStatus] nvarchar(max)  NOT NULL,
    [PaymentReference] nvarchar(max)  NOT NULL,
    [BillAccountId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [BillAccountId] in table 'BillAccounts'
ALTER TABLE [dbo].[BillAccounts]
ADD CONSTRAINT [PK_BillAccounts]
    PRIMARY KEY CLUSTERED ([BillAccountId] ASC);
GO

-- Creating primary key on [BillAccountPolicyId] in table 'BillAccountPolicies'
ALTER TABLE [dbo].[BillAccountPolicies]
ADD CONSTRAINT [PK_BillAccountPolicies]
    PRIMARY KEY CLUSTERED ([BillAccountPolicyId] ASC);
GO

-- Creating primary key on [BillingTransactionId] in table 'BillingTransactions'
ALTER TABLE [dbo].[BillingTransactions]
ADD CONSTRAINT [PK_BillingTransactions]
    PRIMARY KEY CLUSTERED ([BillingTransactionId] ASC);
GO

-- Creating primary key on [InstallmentSummaryId] in table 'InstallmentSummaries'
ALTER TABLE [dbo].[InstallmentSummaries]
ADD CONSTRAINT [PK_InstallmentSummaries]
    PRIMARY KEY CLUSTERED ([InstallmentSummaryId] ASC);
GO

-- Creating primary key on [InstallmentId] in table 'Installments'
ALTER TABLE [dbo].[Installments]
ADD CONSTRAINT [PK_Installments]
    PRIMARY KEY CLUSTERED ([InstallmentId] ASC);
GO

-- Creating primary key on [InvoiceId] in table 'Invoices'
ALTER TABLE [dbo].[Invoices]
ADD CONSTRAINT [PK_Invoices]
    PRIMARY KEY CLUSTERED ([InvoiceId] ASC);
GO

-- Creating primary key on [InvoiceInstallmentId] in table 'InvoiceInstallments'
ALTER TABLE [dbo].[InvoiceInstallments]
ADD CONSTRAINT [PK_InvoiceInstallments]
    PRIMARY KEY CLUSTERED ([InvoiceInstallmentId] ASC);
GO

-- Creating primary key on [PaymentId] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [PK_Payments]
    PRIMARY KEY CLUSTERED ([PaymentId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [BillAccountId] in table 'BillAccountPolicies'
ALTER TABLE [dbo].[BillAccountPolicies]
ADD CONSTRAINT [FK_BillAccountBillAccountPolicy]
    FOREIGN KEY ([BillAccountId])
    REFERENCES [dbo].[BillAccounts]
        ([BillAccountId])
    ON DELETE CASCADE ON UPDATE  CASCADE;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BillAccountBillAccountPolicy'
CREATE INDEX [IX_FK_BillAccountBillAccountPolicy]
ON [dbo].[BillAccountPolicies]
    ([BillAccountId]);
GO

-- Creating foreign key on [BillAccountId] in table 'BillingTransactions'
ALTER TABLE [dbo].[BillingTransactions]
ADD CONSTRAINT [FK_BillAccountBillingTransaction]
    FOREIGN KEY ([BillAccountId])
    REFERENCES [dbo].[BillAccounts]
        ([BillAccountId])
    ON DELETE CASCADE ON UPDATE  CASCADE;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BillAccountBillingTransaction'
CREATE INDEX [IX_FK_BillAccountBillingTransaction]
ON [dbo].[BillingTransactions]
    ([BillAccountId]);
GO

-- Creating foreign key on [BillAccountId] in table 'InstallmentSummaries'
ALTER TABLE [dbo].[InstallmentSummaries]
ADD CONSTRAINT [FK_BillAccountInstallmentSummary]
    FOREIGN KEY ([BillAccountId])
    REFERENCES [dbo].[BillAccounts]
        ([BillAccountId])
    ON DELETE CASCADE ON UPDATE  CASCADE;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BillAccountInstallmentSummary'
CREATE INDEX [IX_FK_BillAccountInstallmentSummary]
ON [dbo].[InstallmentSummaries]
    ([BillAccountId]);
GO

-- Creating foreign key on [BillAccountId] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [FK_BillAccountPayment]
    FOREIGN KEY ([BillAccountId])
    REFERENCES [dbo].[BillAccounts]
        ([BillAccountId])
    ON DELETE CASCADE ON UPDATE  CASCADE;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BillAccountPayment'
CREATE INDEX [IX_FK_BillAccountPayment]
ON [dbo].[Payments]
    ([BillAccountId]);
GO

-- Creating foreign key on [InstallmentSummaryId] in table 'Installments'
ALTER TABLE [dbo].[Installments]
ADD CONSTRAINT [FK_InstallmentSummaryInstallment]
    FOREIGN KEY ([InstallmentSummaryId])
    REFERENCES [dbo].[InstallmentSummaries]
        ([InstallmentSummaryId])
   ON DELETE CASCADE ON UPDATE  CASCADE;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InstallmentSummaryInstallment'
CREATE INDEX [IX_FK_InstallmentSummaryInstallment]
ON [dbo].[Installments]
    ([InstallmentSummaryId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
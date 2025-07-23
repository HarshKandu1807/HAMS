IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Departments] (
    [DepartmentId] int NOT NULL IDENTITY,
    [DepartmentName] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([DepartmentId])
);

CREATE TABLE [Patients] (
    [PatientId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [ContactNo] int NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [Gender] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Patients] PRIMARY KEY ([PatientId])
);

CREATE TABLE [Doctors] (
    [DoctorId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [DepartmentId] int NOT NULL,
    [Specialization] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Doctors] PRIMARY KEY ([DoctorId]),
    CONSTRAINT [FK_Doctors_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([DepartmentId]) ON DELETE CASCADE
);

CREATE TABLE [DoctorLeaves] (
    [LeaveId] int NOT NULL IDENTITY,
    [DoctorId] int NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [Reason] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_DoctorLeaves] PRIMARY KEY ([LeaveId]),
    CONSTRAINT [FK_DoctorLeaves_Doctors_DoctorId] FOREIGN KEY ([DoctorId]) REFERENCES [Doctors] ([DoctorId]) ON DELETE CASCADE
);

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [UserName] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [ContactNo] int NOT NULL,
    [DoctorId] int NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId]),
    CONSTRAINT [FK_Users_Doctors_DoctorId] FOREIGN KEY ([DoctorId]) REFERENCES [Doctors] ([DoctorId])
);

CREATE TABLE [Appointments] (
    [AppointmentId] int NOT NULL IDENTITY,
    [AppointmentDate] datetime2 NOT NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [PatientId] int NOT NULL,
    [DoctorId] int NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [Notes] nvarchar(max) NOT NULL,
    [PrescriptionId] int NULL,
    CONSTRAINT [PK_Appointments] PRIMARY KEY ([AppointmentId]),
    CONSTRAINT [FK_Appointments_Doctors_DoctorId] FOREIGN KEY ([DoctorId]) REFERENCES [Doctors] ([DoctorId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Appointments_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([PatientId]) ON DELETE CASCADE
);

CREATE TABLE [Prescriptions] (
    [PrescriptionId] int NOT NULL IDENTITY,
    [PrescriptionName] nvarchar(max) NOT NULL,
    [AppointmentId] int NOT NULL,
    CONSTRAINT [PK_Prescriptions] PRIMARY KEY ([PrescriptionId]),
    CONSTRAINT [FK_Prescriptions_Appointments_AppointmentId] FOREIGN KEY ([AppointmentId]) REFERENCES [Appointments] ([AppointmentId]) ON DELETE CASCADE
);

CREATE INDEX [IX_Appointments_DoctorId] ON [Appointments] ([DoctorId]);

CREATE INDEX [IX_Appointments_PatientId] ON [Appointments] ([PatientId]);

CREATE INDEX [IX_Appointments_PrescriptionId] ON [Appointments] ([PrescriptionId]);

CREATE INDEX [IX_DoctorLeaves_DoctorId] ON [DoctorLeaves] ([DoctorId]);

CREATE INDEX [IX_Doctors_DepartmentId] ON [Doctors] ([DepartmentId]);

CREATE INDEX [IX_Prescriptions_AppointmentId] ON [Prescriptions] ([AppointmentId]);

CREATE INDEX [IX_Users_DoctorId] ON [Users] ([DoctorId]);

ALTER TABLE [Appointments] ADD CONSTRAINT [FK_Appointments_Prescriptions_PrescriptionId] FOREIGN KEY ([PrescriptionId]) REFERENCES [Prescriptions] ([PrescriptionId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250721071718_init', N'9.0.6');

ALTER TABLE [Appointments] DROP CONSTRAINT [FK_Appointments_Prescriptions_PrescriptionId];

ALTER TABLE [Users] DROP CONSTRAINT [FK_Users_Doctors_DoctorId];

DROP INDEX [IX_Users_DoctorId] ON [Users];

DROP INDEX [IX_Prescriptions_AppointmentId] ON [Prescriptions];

DROP INDEX [IX_Appointments_PrescriptionId] ON [Appointments];

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'DoctorId');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Users] DROP COLUMN [DoctorId];

CREATE UNIQUE INDEX [IX_Prescriptions_AppointmentId] ON [Prescriptions] ([AppointmentId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250721120419_init1', N'9.0.6');

COMMIT;
GO


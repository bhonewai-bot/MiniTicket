create table Tbl_Login
(
    LoginId          int identity
        primary key,
    UserId           int          not null,
    SessionId        varchar(255) not null,
    SessionExpiredAt datetime     not null,
    CreatedAt        datetime default getdate()
)
go

create table Tbl_Ticket
(
    TicketId    int identity
        primary key,
    Title       varchar(100) not null,
    Description varchar(max) not null,
    Status      varchar(20)  not null
        check ([Status] = 'Closed' OR [Status] = 'Resolved' OR [Status] = 'InProgress' OR [Status] = 'Open'),
    UserId      int          not null,
    CreatedAt   datetime default getdate(),
    UpdatedAt   datetime
)
go

create table Tbl_TicketComment
(
    TicketCommentId int identity
        primary key,
    TicketId        int          not null,
    UserId          int          not null,
    Message         varchar(max) not null,
    CreatedAt       datetime default getdate(),
    AuthorRole      varchar(20)
        check ([AuthorRole] = 'Admin' OR [AuthorRole] = 'User')
)
go

create table Tbl_User
(
    UserId    int identity
        primary key,
    Name      varchar(100) not null,
    Email     varchar(100) not null
        unique,
    Password  varchar(255) not null,
    Role      varchar(20)  not null
        check ([Role] = 'Admin' OR [Role] = 'User'),
    CreatedAt datetime default getdate()
)
go


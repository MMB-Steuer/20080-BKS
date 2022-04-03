Create Table Log (
	ID int Identity(1,1),
	createDate DateTime2 not null,
	type varchar(max),
	stacktrace varchar(max),
	messageText varchar(max),
	hint varchar(max)
);
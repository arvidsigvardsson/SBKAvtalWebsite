drop schema if exists sbkavtal cascade;

create schema sbkavtal;


drop table if exists sbkavtal.avtal;
drop table if exists sbkavtal.person;
drop table if exists sbkavtal.intaktskontering;
drop table if exists sbkavtal.avtalsinnehåll;

drop type if exists sbkavtal.avtalsstatus;
create type sbkavtal.avtalsstatus as enum('Aktivt', 'Inaktivt');

drop type if exists sbkavtal.motpartstyp;
create type sbkavtal.motpartstyp as enum('Extern', 'Förvaltning', 'Kommunalt bolag', 'Uppgift saknas');

drop type if exists sbkavtal.avtalsinnehallstyp;
create type sbkavtal.avtalsinnehallstyp as enum('avtalsinnehåll 1', 'avtalsinnehåll 2', 'avtalsinnehåll 3');

create table sbkavtal.person(
	id			serial primary key,
	first_name		varchar(50),
	last_name		varchar(50),
	belagenhetsadress	varchar(50),
	postnummer		varchar(20),
	postort			varchar(50),
	tfn_nummer		varchar(20),
	epost			varchar(50)
);

create table sbkavtal.fakturaadress(
	id			serial primary key,
	first_name		varchar,
	last_name		varchar,
	belagenhetsadress	varchar,
	postnummer		varchar,
	postort			varchar,
	referens		varchar
);

create table sbkavtal.avtal(
	id			serial primary key,
	diarienummer		bigint,
	status			avtalsstatus,
	startdate		date,
	enddate			date,
	orgnummer		varchar(20),

	motpartstyp		motpartstyp,
	SBKavtalsid		int,
	scan_url		varchar(512),
	enligt_avtal		text,
	internt_alias		text,
	kommentar		text,

	avtalstecknare		integer references sbkavtal.person,
	avtalskontakt		integer references sbkavtal.person,

	upphandlat_av		integer references sbkavtal.person,
	ansvarig_SBK		integer references sbkavtal.person,
	ansvarig_avd		varchar(50),
	ansvarig_enhet		varchar(50),

-- 	avtalsinnehall		text,
	avtalsvarde		bigint,

	datakontakt		integer references sbkavtal.person,

-- 	intaktskontering	integer references sbkavtal.intaktskontering,
	konto			varchar,
	kstl			varchar,
	vht			varchar,
	mtp			varchar,
	aktivitet		varchar,
	objekt			varchar,
	projekt			varchar,
	
	fakt_adress		integer references sbkavtal.fakturaadress,

	vitalt_avtal		boolean,
	gallringsår		date
	
);

create table sbkavtal.avtalsinnehall(
	id			serial primary key,
	innehall		sbkavtal.avtalsinnehallstyp,
	avtalsid		integer references avtal not null
);

-- insert into sbkavtal.person(first_name, last_name, belagenhetsadress, postnummer, postort, tfn_nummer, epost)
-- values	('Sven', 'Andersson', 'Svedalavägen', '111 11', 'Svedala', '010-100100', 'sven@example.com');
-- 
-- insert into sbkavtal.avtal(diarienummer, startdate, enddate, orgnummer, status, avtalstecknare, avtalskontakt)
-- values	(314, '2017-01-01', CURRENT_DATE, '820403', 'Aktivt', 1, 1);
-- -- 	(1000, CURRENT_DATE, CURRENT_DATE, '820403', 'Aktivt'),
-- -- 	(666, '1982-04-03', CURRENT_DATE, '140807', 'Inaktivt');
-- 
-- select * from sbkavtal.avtal inner join sbkavtal.person
-- on sbkavtal.avtal.avtalstecknare = sbkavtal.person.id;
-- --select * from person;
-- 
-- insert into sbkavtal.avtalsinnehall(innehall, avtalsid)
-- select 'avtalsinnehåll 1', id from sbkavtal.avtal where diarienummer = 314;
-- 
-- 
-- 
-- select * from sbkavtal.avtalsinnehall;


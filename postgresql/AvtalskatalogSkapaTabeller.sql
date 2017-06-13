-- drop schema if exists sbk_avtal cascade;

-- create schema sbk_avtal;


drop table if exists sbk_avtal.avtal cascade;
drop table if exists sbk_avtal.person cascade;
-- drop table if exists sbk_avtal.intaktskontering cascade;
drop table if exists sbk_avtal.avtalsinnehall cascade;
drop table if exists sbk_avtal.fakturaadress cascade;
drop table if exists sbk_avtal.map_avtal_innehall cascade;
drop table if exists sbk_avtal.log_updates cascade;


drop type if exists sbk_avtal.avtalsstatus;
create type sbk_avtal.avtalsstatus as enum('Aktivt', 'Inaktivt');

drop type if exists sbk_avtal.motpartstyp;
create type sbk_avtal.motpartstyp as enum('Extern', 'Förvaltning', 'Kommunalt bolag', 'Uppgift saknas');

-- drop type if exists sbk_avtal.avtalsinnehallstyp;
-- create type sbk_avtal.avtalsinnehallstyp as enum('avtalsinnehåll 1', 'avtalsinnehåll 2', 'avtalsinnehåll 3');

create table sbk_avtal.person(
	id			serial primary key,
	first_name		varchar(50),
	last_name		varchar(50),
	belagenhetsadress	varchar(50),
	postnummer		varchar(20),
	postort			varchar(50),
	tfn_nummer		varchar(20),
	epost			varchar(50)
);

create table sbk_avtal.fakturaadress(
	id			serial primary key,
	first_name		varchar,
	last_name		varchar,
	belagenhetsadress	varchar,
	postnummer		varchar,
	postort			varchar,
	referens		varchar
);

create table sbk_avtal.avtal(
	id			serial primary key,
	diarienummer		bigint,
	status			sbk_avtal.avtalsstatus,
	startdate		date,
	enddate			date,
	orgnummer		varchar(20),

	motpartstyp		sbk_avtal.motpartstyp,
	SBKavtalsid		int,
	scan_url		varchar(512),
	enligt_avtal		text,
	internt_alias		text,
	kommentar		text,

	avtalstecknare		integer references sbk_avtal.person,
	avtalskontakt		integer references sbk_avtal.person,

	upphandlat_av		integer references sbk_avtal.person,
	ansvarig_SBK		integer references sbk_avtal.person,
	ansvarig_avd		varchar(50),
	ansvarig_enhet		varchar(50),

-- 	avtalsinnehall		text,
	avtalsvarde		bigint,

	datakontakt		integer references sbk_avtal.person,

-- 	intaktskontering	integer references sbk_avtal.intaktskontering,
	konto			varchar,
	kstl			varchar,
	vht			varchar,
	mtp			varchar,
	aktivitet		varchar,
	objekt			varchar,
	projekt			varchar,
	
	fakt_adress		integer references sbk_avtal.fakturaadress,

	vitalt_avtal		boolean,
	gallringsår		date
	
);

create table sbk_avtal.avtalsinnehall(
	id			serial primary key,
	beskrivning		varchar
	-- här kan läggas till fält för exempelvis värde på innehållet
);

create table sbk_avtal.map_avtal_innehall(
	id			serial primary key,
	avtal_id		integer references sbk_avtal.avtal,
	avtalsinnehall_id	integer references sbk_avtal.avtalsinnehall
);

--tabell som loggar uppdateringar av avtalstabellen, med samma kolumner plus användarnamn för den som uppdaterar samt timestamp
create table sbk_avtal.log_updates(
	id			serial primary key,

	avtal_id		integer references sbk_avtal.avtal,
	uppdaterat_av		varchar,
	created_at		timestamp default now(),
	
	diarienummer		bigint,
	status			sbk_avtal.avtalsstatus,
	startdate		date,
	enddate			date,
	orgnummer		varchar(20),

	motpartstyp		sbk_avtal.motpartstyp,
	SBKavtalsid		int,
	scan_url		varchar(512),
	enligt_avtal		text,
	internt_alias		text,
	kommentar		text,

	avtalstecknare		integer references sbk_avtal.person,
	avtalskontakt		integer references sbk_avtal.person,

	upphandlat_av		integer references sbk_avtal.person,
	ansvarig_SBK		integer references sbk_avtal.person,
	ansvarig_avd		varchar(50),
	ansvarig_enhet		varchar(50),

-- 	avtalsinnehall		text,
	avtalsvarde		bigint,

	datakontakt		integer references sbk_avtal.person,

-- 	intaktskontering	integer references sbk_avtal.intaktskontering,
	konto			varchar,
	kstl			varchar,
	vht			varchar,
	mtp			varchar,
	aktivitet		varchar,
	objekt			varchar,
	projekt			varchar,
	
	fakt_adress		integer references sbk_avtal.fakturaadress,

	vitalt_avtal		boolean,
	gallringsår		date
);



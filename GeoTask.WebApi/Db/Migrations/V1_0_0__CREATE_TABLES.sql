--
-- PostgreSQL database dump
--

-- Dumped from database version 12.1
-- Dumped by pg_dump version 12.1


--
-- Name: geo_data_en; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.geo_data_en (
    time_zone character varying(50),
    metro_code integer,
    geoname_id bigint NOT NULL,
    country_name character varying(100),
    country_iso_code character(2) NOT NULL,
    continent_name character varying(100),
    continent_code character(2) NOT NULL,
    city_name character varying(100),
    is_in_european_union boolean NOT NULL
);

--
-- Name: geo_data_de; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.geo_data_de (
    time_zone character varying(50),
    metro_code integer,
    geoname_id bigint NOT NULL,
    country_name character varying(100),
    country_iso_code character(2) NOT NULL,
    continent_name character varying(100),
    continent_code character(2) NOT NULL,
    city_name character varying(100),
    is_in_european_union boolean NOT NULL
);

--
-- Name: geo_data_es; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.geo_data_es (
    time_zone character varying(50),
    metro_code integer,
    geoname_id bigint NOT NULL,
    country_name character varying(100),
    country_iso_code character(2) NOT NULL,
    continent_name character varying(100),
    continent_code character(2) NOT NULL,
    city_name character varying(100),
    is_in_european_union boolean NOT NULL
);

--
-- Name: geo_data_fr; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.geo_data_fr (
    time_zone character varying(50),
    metro_code integer,
    geoname_id bigint NOT NULL,
    country_name character varying(100),
    country_iso_code character(2) NOT NULL,
    continent_name character varying(100),
    continent_code character(2) NOT NULL,
    city_name character varying(100),
    is_in_european_union boolean NOT NULL
);

--
-- Name: geo_data_ja; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.geo_data_ja (
    time_zone character varying(50),
    metro_code integer,
    geoname_id bigint NOT NULL,
    country_name character varying(100),
    country_iso_code character(2) NOT NULL,
    continent_name character varying(100),
    continent_code character(2) NOT NULL,
    city_name character varying(100),
    is_in_european_union boolean NOT NULL
);

--
-- Name: geo_data_pt-BR; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."geo_data_pt-BR" (
    time_zone character varying(50),
    metro_code integer,
    geoname_id bigint NOT NULL,
    country_name character varying(100),
    country_iso_code character(2) NOT NULL,
    continent_name character varying(100),
    continent_code character(2) NOT NULL,
    city_name character varying(100),
    is_in_european_union boolean NOT NULL
);

--
-- Name: geo_data_ru; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.geo_data_ru (
    time_zone character varying(50),
    metro_code integer,
    geoname_id bigint NOT NULL,
    country_name character varying(100),
    country_iso_code character(2) NOT NULL,
    continent_name character varying(100),
    continent_code character(2) NOT NULL,
    city_name character varying(100),
    is_in_european_union boolean NOT NULL
);

--
-- Name: geo_data_zh-CN; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."geo_data_zh-CN" (
    time_zone character varying(50),
    metro_code integer,
    geoname_id bigint NOT NULL,
    country_name character varying(100),
    country_iso_code character(2) NOT NULL,
    continent_name character varying(100),
    continent_code character(2) NOT NULL,
    city_name character varying(100),
    is_in_european_union boolean NOT NULL
);

--
-- Name: ip; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ip (
    network inet NOT NULL,
    longitude character varying(25),
    latitude character varying(25),
    geoname_id bigint NOT NULL,
    accuracy_radius integer
);

--
-- Name: changelog id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.geo_data_en
    ADD CONSTRAINT geo_data_en_pkey PRIMARY KEY (geoname_id);

ALTER TABLE ONLY public.geo_data_de
    ADD CONSTRAINT geo_data_de_pkey PRIMARY KEY (geoname_id);

ALTER TABLE ONLY public.geo_data_es
    ADD CONSTRAINT geo_data_es_pkey PRIMARY KEY (geoname_id);

ALTER TABLE ONLY public.geo_data_fr
    ADD CONSTRAINT geo_data_fr_pkey PRIMARY KEY (geoname_id);

ALTER TABLE ONLY public.geo_data_ja
    ADD CONSTRAINT geo_data_ja_pkey PRIMARY KEY (geoname_id);

ALTER TABLE ONLY public."geo_data_pt-BR"
    ADD CONSTRAINT geo_data_pt_pkey PRIMARY KEY (geoname_id);

ALTER TABLE ONLY public.geo_data_ru
    ADD CONSTRAINT geo_data_ru_pkey PRIMARY KEY (geoname_id);

ALTER TABLE ONLY public."geo_data_zh-CN"
    ADD CONSTRAINT geo_data_zh_pkey PRIMARY KEY (geoname_id);

--
-- Name: ip ip_v4_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ip
    ADD CONSTRAINT ip_v4_pkey PRIMARY KEY (network);


--
-- PostgreSQL database dump complete
--
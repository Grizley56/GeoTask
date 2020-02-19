--
-- PostgreSQL database dump
--

-- Dumped from database version 12.1
-- Dumped by pg_dump version 12.1

CREATE TABLE public.location
(
    time_zone character varying(50) COLLATE pg_catalog."default",
    metro_code integer,
    is_in_european_union boolean NOT NULL,
    geoname_id bigint NOT NULL,
    country_name character varying(100) COLLATE pg_catalog."default",
    country_iso_code character varying(2) COLLATE pg_catalog."default",
    continent_name character varying(100) COLLATE pg_catalog."default",
    continent_code character varying(2) COLLATE pg_catalog."default",
    city_name character varying(100) COLLATE pg_catalog."default",
    locale_code character varying(7) COLLATE pg_catalog."default" NOT NULL
)

TABLESPACE pg_default;

ALTER TABLE public.location
    OWNER to postgres;

-- Index: location_geoname_id_index

CREATE INDEX location_geoname_id_index
    ON public.location USING hash
    (geoname_id)
    TABLESPACE pg_default;

-- Index: locations_locale_code_index

CREATE INDEX locations_locale_code_index
    ON public.location USING hash
    (locale_code COLLATE pg_catalog."C" varchar_ops)
    TABLESPACE pg_default;


CREATE TABLE public.ip
(
    network inet NOT NULL,
    longitude character varying(25) COLLATE pg_catalog."default",
    latitude character varying(25) COLLATE pg_catalog."default",
    geoname_id bigint NOT NULL,
    accuracy_radius integer,
    CONSTRAINT __ip_pkey PRIMARY KEY (network)
)

TABLESPACE pg_default;

ALTER TABLE public.ip
    OWNER to postgres;

-- Index: ip_geoname_id_index

CREATE INDEX ip_geoname_id_index
    ON public.ip USING btree
    (geoname_id)
    TABLESPACE pg_default;

--
-- PostgreSQL database dump complete
--
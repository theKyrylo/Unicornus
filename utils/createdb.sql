CREATE TABLE continents (
  continent_id SERIAL PRIMARY KEY,
  continent_name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE countries (
  country_id SERIAL PRIMARY KEY,
  country_name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE locations (
  location_id SERIAL PRIMARY KEY,
  city VARCHAR(100) NOT NULL,
  country_id INTEGER NOT NULL,
  continent_id INTEGER NOT NULL,
  CONSTRAINT fk_country
    FOREIGN KEY (country_id)
    REFERENCES countries (country_id)
    ON DELETE RESTRICT,
  CONSTRAINT fk_continent
    FOREIGN KEY (continent_id)
    REFERENCES continents (continent_id)
    ON DELETE RESTRICT,
  UNIQUE (city, country_id)
);

CREATE TABLE industries (
  industry_id SERIAL PRIMARY KEY,
  industry_name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE investors (
  investor_id SERIAL PRIMARY KEY,
  investor_name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE companies (
  company_id SERIAL PRIMARY KEY,
  company_name VARCHAR(100) NOT NULL UNIQUE,
  year_founded INTEGER,
  valuation_in_billions NUMERIC(15, 2),
  date_joined_unicorn DATE,
  funding_amount NUMERIC(20, 2),
  funding_unit CHAR(1) CHECK (funding_unit IN ('M', 'B')),
  location_id INTEGER NOT NULL,
  industry_id INTEGER NOT NULL,
  CONSTRAINT fk_location
  FOREIGN KEY (location_id)
  REFERENCES locations (location_id)
  ON DELETE RESTRICT,
  CONSTRAINT fk_industry
  FOREIGN KEY (industry_id)
  REFERENCES industries (industry_id)
  ON DELETE RESTRICT
);

CREATE TABLE company_investors (
  company_id INTEGER NOT NULL,
  investor_id INTEGER NOT NULL,
  PRIMARY KEY (company_id, investor_id),
  CONSTRAINT fk_company
    FOREIGN KEY (company_id)
    REFERENCES companies (company_id)
    ON DELETE CASCADE,
  CONSTRAINT fk_investor
    FOREIGN KEY (investor_id)
    REFERENCES investors (investor_id)
    ON DELETE CASCADE
);

CREATE INDEX idx_companies_valuation ON companies (valuation_in_billions);
CREATE INDEX idx_companies_year_founded ON companies (year_founded);
CREATE INDEX idx_companies_date_joined ON companies (date_joined_unicorn);
CREATE INDEX idx_locations_country ON locations (country_id);
CREATE INDEX idx_locations_continent ON locations (continent_id);
CREATE INDEX idx_company_investors_investor ON company_investors (investor_id);

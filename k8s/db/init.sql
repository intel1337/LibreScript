-- Create the test user
CREATE USER testuser WITH PASSWORD 'testpass';

-- Grant privileges to the user on the database
GRANT ALL PRIVILEGES ON DATABASE your_db TO testuser;

-- Grant schema privileges
GRANT ALL PRIVILEGES ON SCHEMA public TO testuser;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO testuser;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO testuser; 
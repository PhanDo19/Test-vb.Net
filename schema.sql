CREATE TABLE performances (
    id SERIAL PRIMARY KEY,
    title TEXT NOT NULL,
    starts_at TIMESTAMPTZ NOT NULL,
    duration_minutes INT NOT NULL CHECK (duration_minutes > 0),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TYPE seat_category AS ENUM ('standard','vip','double');

CREATE TABLE bookings (
    id SERIAL PRIMARY KEY,
    performance_id INT NOT NULL REFERENCES performances(id) ON DELETE CASCADE,
    customer_name TEXT NOT NULL,
    category seat_category NOT NULL,
    ticket_count INT NOT NULL CHECK (ticket_count > 0),
    total_amount NUMERIC(12,2) NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE seat_assignments (
    id SERIAL PRIMARY KEY,
    booking_id INT NOT NULL REFERENCES bookings(id) ON DELETE CASCADE,
    performance_id INT NOT NULL REFERENCES performances(id) ON DELETE CASCADE,
    seat_row CHAR(1) NOT NULL CHECK (seat_row BETWEEN 'A' AND 'J'),
    seat_number INT NOT NULL CHECK (seat_number BETWEEN 1 AND 10),
    UNIQUE (performance_id, seat_row, seat_number),
    UNIQUE (booking_id, seat_row, seat_number)
);

CREATE VIEW v_booking_seats AS
SELECT b.id AS booking_id, b.customer_name, b.category, b.ticket_count,
       p.title, p.starts_at,
       sa.seat_row || sa.seat_number::text AS seat_code
FROM bookings b
JOIN performances p ON p.id = b.performance_id
LEFT JOIN seat_assignments sa ON sa.booking_id = b.id;


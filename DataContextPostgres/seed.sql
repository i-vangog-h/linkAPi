CREATE TABLE url
(
    id SERIAL PRIMARY KEY,
    hash VARCHAR(10) NOT NULL,
    original_url TEXT NOT NULL,
    created_at TIMESTAMP,
    access_count INT DEFAULT 0,
    CONSTRAINT url_pkey PRIMARY KEY (id),
    CONSTRAINT url_original_url_key UNIQUE (original_url)
);

INSERT INTO url(hash, original_url, created_at)
	VALUES 
        ("a", "https://www.youtube.com/watch?v=l_7JA-4UVvI&t=2542s&ab_channel=Jordaan", CURRENT_TIMESTAMP),
	    ("b", "https://soundcloud.com/ivangogh", CURRENT_TIMESTAMP);
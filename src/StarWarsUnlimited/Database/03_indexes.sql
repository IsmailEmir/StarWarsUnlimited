-- ============================================================
-- Индексы
-- Сгенерировано автоматически из docs/dbml/schema.dbml
-- ============================================================

BEGIN;

CREATE INDEX IF NOT EXISTS "idx_cards_title"
    ON "cards" ("title");

CREATE INDEX IF NOT EXISTS "idx_cards_set_id"
    ON "cards" ("set_id");

CREATE INDEX IF NOT EXISTS "idx_cards_rarity_id"
    ON "cards" ("rarity_id");

CREATE INDEX IF NOT EXISTS "idx_cards_type_id"
    ON "cards" ("type_id");

CREATE INDEX IF NOT EXISTS "idx_cards_cost"
    ON "cards" ("cost");

CREATE INDEX IF NOT EXISTS "idx_cards_card_number"
    ON "cards" ("card_number");

CREATE INDEX IF NOT EXISTS "idx_card_aspects_aspect_id"
    ON "card_aspects" ("aspect_id");

CREATE INDEX IF NOT EXISTS "idx_card_traits_trait_id"
    ON "card_traits" ("trait_id");

CREATE INDEX IF NOT EXISTS "idx_comments_card_id"
    ON "comments" ("card_id");

COMMIT;

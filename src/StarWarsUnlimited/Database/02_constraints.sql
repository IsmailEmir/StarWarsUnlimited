-- ============================================================
-- Внешние ключи (Ref)
-- Сгенерировано автоматически из docs/dbml/schema.dbml
-- ============================================================

BEGIN;

ALTER TABLE "cards" ADD CONSTRAINT "fk_cards_set_id"
    FOREIGN KEY ("set_id")
    REFERENCES "sets" ("id")
;

ALTER TABLE "cards" ADD CONSTRAINT "fk_cards_rarity_id"
    FOREIGN KEY ("rarity_id")
    REFERENCES "rarities" ("id")
;

ALTER TABLE "cards" ADD CONSTRAINT "fk_cards_type_id"
    FOREIGN KEY ("type_id")
    REFERENCES "card_types" ("id")
;

ALTER TABLE "card_aspects" ADD CONSTRAINT "fk_card_aspects_card_id"
    FOREIGN KEY ("card_id")
    REFERENCES "cards" ("id")
    ON DELETE CASCADE
;

ALTER TABLE "card_aspects" ADD CONSTRAINT "fk_card_aspects_aspect_id"
    FOREIGN KEY ("aspect_id")
    REFERENCES "aspects" ("id")
    ON DELETE CASCADE
;

ALTER TABLE "card_traits" ADD CONSTRAINT "fk_card_traits_card_id"
    FOREIGN KEY ("card_id")
    REFERENCES "cards" ("id")
    ON DELETE CASCADE
;

ALTER TABLE "card_traits" ADD CONSTRAINT "fk_card_traits_trait_id"
    FOREIGN KEY ("trait_id")
    REFERENCES "traits" ("id")
    ON DELETE CASCADE
;

ALTER TABLE "comments" ADD CONSTRAINT "fk_comments_card_id"
    FOREIGN KEY ("card_id")
    REFERENCES "cards" ("id")
    ON DELETE CASCADE
;

COMMIT;

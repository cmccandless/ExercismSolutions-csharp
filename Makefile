ifeq ($(OS),Windows_NT)
	DOTNET=@cmd /C dotnet
else
	DOTNET=dotnet
endif

EXTENSION:=cs
SOURCE_FILES := $(shell find */* -maxdepth 1 -type f -name '*.$(EXTENSION)')
EXERCISES := $(shell find */* -maxdepth 1 -type f -name '*.$(EXTENSION)' | cut -d/ -f1 | uniq)
OUT_DIR=.build
OBJECTS=$(addprefix $(OUT_DIR)/,$(EXERCISES))
CLEAN_TARGETS:=$(addprefix clean-,$(EXERCISES))
MIGRATE_OBJECTS :=$(addsuffix /.exercism/metadata.json, $(EXERCISES))

.PHONY: all test clean check-migrate no-skip
all: test
pre-push pre-commit: no-skip check-migrate test

no-skip:
	@ echo "$@: TODO"

check-migrate: $(MIGRATE_OBJECTS)

$(MIGRATE_OBJECTS):
	@ [ -f $@ ] || $(error "$(shell echo $@ | cut -d/ -f1) has not been migrated")

clean: $(CLEAN_TARGETS)
$(CLEAN_TARGETS):
	$(eval EXERCISE := $(patsubst $(OUT_DIR)/%,%,$@))
	rm -rf $(EXERCISE)/bin $(EXERCISE)/obj $(OUT_DIR)/$(EXERCISE)

test: $(EXERCISES)
$(EXERCISES): %: $(OUT_DIR)/%

$(OUT_DIR):
	@ mkdir -p $@

.SECONDEXPANSION:

GET_DEP = $(filter $(patsubst $(OUT_DIR)/%,%,$@)%,$(SOURCE_FILES))
$(OBJECTS): $$(GET_DEP) | $(OUT_DIR)
	$(eval EXERCISE := $(patsubst $(OUT_DIR)/%,%,$@))
	@ echo "Testing $(EXERCISE)..."
	@ DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 $(DOTNET) test $(OPTS) $(EXERCISE)
	@ touch $@

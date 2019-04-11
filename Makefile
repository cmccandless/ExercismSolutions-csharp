ifeq ($(OS),Windows_NT)
	DOTNET=@cmd /C dotnet
else
	DOTNET=dotnet
endif

TRACK:=$(shell basename $(shell pwd))
EXTENSION:=cs
SOURCE_FILES := $(shell find */* -maxdepth 1 -type f -name '*.$(EXTENSION)')
EXERCISES := $(shell find */* -maxdepth 1 -type f -name '*.$(EXTENSION)' | cut -d/ -f1 | uniq)
OUT_DIR=.build
OBJECTS=$(addprefix $(OUT_DIR)/,$(EXERCISES))
CLEAN_TARGETS:=$(addprefix clean-,$(EXERCISES))
MIGRATE_OBJECTS :=$(addsuffix /.exercism/metadata.json, $(EXERCISES))
MIGRATE_TARGETS:=$(addprefix migrate-,$(EXERCISES))

.PHONY: all test clean no-skip
all: test
pre-push pre-commit: no-skip test

no-skip:
	@ ! grep -rP '(?<=Fact)\(Skip = "Remove to run test"\)' --exclude-dir=bin/ .

migrate: $(MIGRATE_TARGETS)
$(MIGRATE_TARGETS): migrate-%: %/.exercism/metadata.json

$(MIGRATE_OBJECTS):
	$(eval EXERCISE := $(patsubst %/.exercism/metadata.json,%,$@))
	exercism download -t $(TRACK) -e $(EXERCISE)
	ls $(EXERCISE)/*.$(EXTENSION) | xargs -n1 git checkout --

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

ifeq ($(OS),Windows_NT)
	DOTNET=@cmd /C dotnet
else
	DOTNET=dotnet
endif

.PHONY: lint test
lint:
	@echo "No linter configured"

# test:
# 	$(DOTNET) test $(OPTS) $(FILES)


test:
	@ $(foreach FILE,$(FILES), \
		$(call dotest,$(FILE)) \
	)

test-all:
	@ $(foreach FILE,$(shell ls -d */), \
		$(call dotest, $(FILE)) \
	)

define dotest
	cd $(1); \
	DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 $(DOTNET) test $(OPTS); \
	cd ..;
endef

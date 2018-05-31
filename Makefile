ifeq ($(OS),Windows_NT)
	DOTNET=@cmd /C dotnet
else
	DOTNET=dotnet
endif

.PHONY: lint test
lint:
	@echo "No linter configured"

test:
	$(DOTNET) test $(OPTS) $(FILES)

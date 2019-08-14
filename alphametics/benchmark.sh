#!/bin/bash

# RUN_LEGACY=1 # 2.4605 Minutes
SOLUTION_FILE=alphametics/Alphametics.cs


get_variable()
{
    local VAR_NAME="$1"
    grep -oP '(?<='"$VAR_NAME"' = )([0-9]+)' "$SOLUTION_FILE"
}

set_variable()
{
    local VAR_NAME="$1"
    local VAR_VALUE="$2"
    sed -i 's/'"$VAR_NAME"' = .*;/'"$VAR_NAME"' = '"$VAR_VALUE"';/g' "$SOLUTION_FILE"
}

print_row()
{
    local fmt='%10s%61s%24s%16s'
    local end='\n'
    if [ -n "$5" ]; then
        end='\r'
    fi
    printf "$fmt$end" "$1" "$2" "$3" "$4" | sed 's/\/\//  /g'
}

run_job()
{
    dotnet test alphametics | grep -oP '(?<=time: )(.*)'
}

find_all()
{
    NAME="$1"
    awk '/#endregion/{f=0} f; /#region '"$NAME"'/{f=1}' "$SOLUTION_FILE" | grep -oP '([^\s].*[^,])'
}

run_matrix()
{
    print_row 'USE_LEGACY' 'PERMUTER' 'CHECKER' 'RUNTIME'

    if [ -n "$RUN_LEGACY" ]; then
        set_variable USE_LEGACY 'true'
        set_variable PERMUTER null
        set_variable CHECKER null
        print_row 'true' 'N/A' 'N/A' "[RUNNING]" 1
        local runtime="$(run_job)"
        print_row 'true' 'N/A' 'N/A' "$runtime"
    else
        print_row 'true' 'N/A' 'N/A' "DISABLED"
    fi

    set_variable USE_LEGACY 'false'
    IFS=$'\n'
    for permuter in $(find_all PERMUTER); do
        grep -qE '^//' <<<"$permuter"
        local permuter_enabled="$?"
        if [ $permuter_enabled -eq 1 ]; then
            set_variable PERMUTER "$permuter"
        fi
        for checker in $(find_all CHECKER); do
            grep -qE '^//' <<<"$checker"
            local checker_enabled="$?"
            if [ $permuter_enabled -eq 1 ] && [ $checker_enabled -eq 1 ]; then
                set_variable CHECKER "AlphameticsDynamic.$checker"
                print_row 'false' "$permuter" "$checker" '[RUNNING]' 1
                local runtime="$(run_job)"
                print_row 'false' "$permuter" "$checker" "$runtime"
            else
                print_row 'false' "$permuter" "$checker" "DISABLED"
            fi
        done
    done
}

print_matrix_info()
{
    if [ -n "$RUN_LEGACY" ]; then
        echo "LEGACY_JOB=enabled"
    else
        echo "LEGACY_JOB=disabled"
    fi
    local PERMUTER_COUNT="$(find_all PERMUTER | wc -l)"
    echo "PERMUTER_COUNT=$PERMUTER_COUNT"
    local CHECKER_COUNT="$(find_all CHECKER | wc -l)"
    echo "CHECKER_COUNT=$CHECKER_COUNT"
    echo
}

print_matrix_info
run_matrix

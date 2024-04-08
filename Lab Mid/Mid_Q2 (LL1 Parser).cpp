#include <iostream>
#include <string>

using namespace std;

string input;
size_t index;

bool match(char expected) {
    if (input[index] == expected) {
        index++;
        return true;
    }
    return false;
}

bool Z();

bool Y_prime() {
    if (input[index] == '&') {
        index++;
        if (Z()) {
            return Y_prime();
        }
        return false;
    }
    return true;
}

bool Y() {
    if (Z()) {
        return Y_prime();
    }
    return false;
}

bool X_prime() {
    if (input[index] == '%') {
        index++;
        if (Y()) {
            return X_prime();
        }
        return false;
    }
    return true;
}

bool X() {
    if (Y()) {
        return X_prime();
    }
    return false;
}

bool S() {
    if (X()) {
        return match('$');
    }
    return false;
}

bool Z() {
    if (input[index] == 'k') {
        index++;
        if (X()) {
            return match('k');
        }
        return false;
    } else if (input[index] == 'g') {
        index++;
        return true;
    }
    return false;
}

bool parse(const string& str) {
    input = str;
    index = 0;
    return S() && index == input.length();
}

int main() {
    string userInput;
    char choice;

    do {
        cout << "Enter a string to parse: ";
        cin >> userInput;

        if (parse(userInput)) {
            cout << "Accepted" << endl;
        } else {
            cout << "Rejected" << endl;
        }

        cout << "Do you want to enter another string? (y/n): ";
        cin >> choice;
    } while (choice == 'y' || choice == 'Y');

    return 0;
}

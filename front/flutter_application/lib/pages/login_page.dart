import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application/pages/home_page.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:http/http.dart' as http;

class LoginPage extends StatefulWidget {
  const LoginPage({super.key});

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  final _formKey = GlobalKey<FormState>();
  final _userController = TextEditingController();
  final _passwordController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Form(
        key: _formKey,
        child: Center(
          child: SingleChildScrollView(
            padding: const EdgeInsets.symmetric(horizontal: 16),
            child: Column(
              children: [
                TextFormField(
                  decoration: const InputDecoration(
                    labelText: 'Username',
                  ),
                  controller: _userController,
                  keyboardType: TextInputType.text,
                  validator: (user) {
                    if (user == null || user.isEmpty) {
                      return 'Please, type your username';
                    }
                    return null;
                  },
                ),
                TextFormField(
                  decoration: const InputDecoration(
                    labelText: 'Password',
                  ),
                  controller: _passwordController,
                  obscureText: true,  // Torna o texto da senha invisível
                  validator: (password) {
                    if (password == null || password.isEmpty) {
                      return "Please type your password";
                    }
                    return null;
                  },
                ),
                ElevatedButton(
                  onPressed: () async {
                    if (_formKey.currentState!.validate()) {
                      FocusScopeNode currentFocus = FocusScope.of(context);
                      bool successfullyLogin = await login();
                      if (!currentFocus.hasPrimaryFocus) {
                        currentFocus.unfocus();
                      }
                      if (successfullyLogin) {
                        Navigator.pushReplacement(
                          context,
                          MaterialPageRoute(builder: (context) => HomePage()),
                        );
                      } else {
                        _passwordController.clear();
                        ScaffoldMessenger.of(context).showSnackBar(snackBar);
                      }
                    }
                  },
                  child: const Text("Login"),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  final snackBar = const SnackBar(content: Text("E-mail or password invalid"));

  Future<bool> login() async {
    SharedPreferences sharedPreference = await SharedPreferences.getInstance();
    try {
      var url = Uri.parse("http://localhost:5210/api/login");
      var response = await http.post(
        url,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'userName': _userController.text,
          'password': _passwordController.text,
        }),
      );

      if (response.statusCode == 200) {
        final token = jsonDecode(response.body)['token'];
        SharedPreferences prefs = await SharedPreferences.getInstance();
        prefs.setString('auth_token', token);
        print(token);
        return true;
      } else {
        print("Error: ${response.statusCode} - ${response.body}");
        return false;
      }
    } catch (e) {
      print("Error: $e");
      return false;
    }
  }
}
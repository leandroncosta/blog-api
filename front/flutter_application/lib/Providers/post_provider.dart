import 'package:flutter/material.dart';

class PostProvider extends ChangeNotifier {
  bool _isDarkTheme = false;
  get isDarktheme => _isDarkTheme;

  changeTheme() {
    _isDarkTheme = !_isDarkTheme;
    notifyListeners();
  }
}

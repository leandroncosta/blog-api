import 'package:flutter/material.dart';
import 'package:flutter_application/Providers/post_provider.dart';
import 'package:provider/provider.dart';

class BottomNavBar extends StatefulWidget {
  const BottomNavBar({super.key});

  @override
  State<BottomNavBar> createState() => _BottomnavigationbarState();
}

class _BottomnavigationbarState extends State<BottomNavBar> {
  late int _selectedIndex=2 ;
  static const TextStyle optionStyle =
      TextStyle(fontSize: 30, fontWeight: FontWeight.bold);

  _onItemTapped(int index) {
    setState(() {
      _selectedIndex = index;
    });
    switch (index) {
      case 0:
        Navigator.of(context).pushReplacementNamed("/login");
        break;
      case 1:
        Navigator.of(context).pushReplacementNamed("/users");
        break;
      case 2:
        Navigator.of(context).pushReplacementNamed("/postview");
        break;
    }
  }

  @override
  Widget build(BuildContext context) {
    
    return BottomNavigationBar(
      showUnselectedLabels: false,
        backgroundColor: context.watch<PostProvider>().isDarktheme
        ? const Color.fromARGB(255, 34, 35, 34)
        : const Color.fromARGB(255, 208, 209, 208),
        currentIndex: _selectedIndex,
        selectedItemColor: const Color.fromARGB(255, 6, 198, 25),
        onTap: _onItemTapped,
        items: const <BottomNavigationBarItem>[
          BottomNavigationBarItem(
            label: 'Login',
            icon: Icon(Icons.login),
          ),
          BottomNavigationBarItem(
            label: 'Usuários',
            icon: Icon(Icons.emoji_people),
          ),
          BottomNavigationBarItem(
            label: 'Posts',
            icon: Icon(Icons.add_a_photo),
          )
        ]);
  }
}
